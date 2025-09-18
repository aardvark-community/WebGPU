# Compute Rasterizer

This project implements a rasterizer fully based on compute shaders in F#.

To efficiently render a scene, the rasterizer splits the screen in tiles ("bins") and determines which triangles overlap which bins. So pixels only have to consider a small amount of triangles for rasterization.
We do this by first transforming all vertices into clip space. Afterwards we split the scene into chunks of triangles and execute the following steps for each chunk:
* Bin dispatch: Calculates the bin-triangle intersection to check which triangle overlaps which bins. Stores a binary number in a large global memory triangle mask of size binCount \* triangleChunkSize. 
* Scan dispatch: Calculates the prefix sum of each row in the triangle mask.
* Compact dispatch: Using the prefix sum, store the triangle indices that each bin needs to consider in a compacted triange mask (Unfortunately, this needs to be the same size as the original triangle mask as all triangles of the scene could lie in a bin).
* Rasterize dispatch: Each pixel knows which bin it belongs and therefore only has to iterate over the triangles that lie in the same bin. Finally, for each triangle the pixel checks via barycentric coordinates if it is a position on the triangle and if the depth test passes before it calculates the color.


## Optimization

The raw version of the above implementation unwillingly leads to an imbalanced GPU load, as in most scenes, triangles are not spread uniformly across the screen but instead might cluster at certain positions. This leads to some bins being overly filled while others are nearly empty.
We counter this issue with two measures.

### GPU load balancing

Initially we created a thread for each pixel. This however leads to imbalances as some pixels have to check many triangles whereas others have 0 or just a few triangles to depth-test and color shade.
Thus, we implement basic GPU load balancing for rasterization. Instead that each GPU thread corresponds to a pixel, we dispatch a fixed number of threads that retrieve their pixel id from an atomic counter. When a thread finishes rasterizing, it increments the atomic counter by one and retrieves the next pixel id until the whole screen is rasterized. This way, threads that retrieve fast rasterizable pixels, can immediately gather a new pixel to work on.

### Quadtree

Due to the nature of our algorithm, we get a tradeoff between binning speed and rasterization speed that depends on the bin size: A large bin size leads to fast binning but slow rasterization, whereas a small bin size leads to the opposite.
We implement a screen-space quadtree structure to spatially load balance the scene by splitting bins that exceed a triangle count threshold into four smaller bins.
This not only means that the bins are no longer uniformly distributed across the screen, but also that our triangle mask matrices are no longer of fixed size.
Therefore, we allocate more space than required and set a fixed maximum number of bins.
Also, we allocate 9 additional integers as header for each row in the compacted triangle mask. The first integer contains the amount of triangles in the bin, the next four integers contain the bounding box of the bin in pixels and the last four integers leave space for possible child ids.
Whenever a bin is split it creates four new bin ids via an atomic counter, stores the ids in header and marks its row in the compacted triangle mask as invalid.

If at least one bin was split, we redo binning, scanning, compactig and splitting until no longer any splits occur or we reach a user set maximum number of splits.

With the quadtree built, we can finally start rasterizing. The only difference in the rasterize dispatch is to find the corresponding child bin (if there are any).
Whenever a pixel is part of an invalid bin it starts traversing the tree by checking in which of the 4 quadrants it lies. The pixel then reads the child id from the header and checks if there are any sub children.


## Build

Follow the steps at [WebGPU](https://github.com/aardvark-community/WebGPU) and finally build the solution.

## Execute

After building the Demo.exe can simply be started (Note that the path to a mesh (.obj) might need to be adapted in the file ComputeRasterizerDemo.fs).
The mesh is loaded at origin and there is a simple camera controller. Currently, the following keybinds change the behavior of the quadtree:
* U/I - decrease/increase the maximum number of splits
* O/P - decrease/increase the starting bin size
* J/K - decrease/increase the split threshold in small steps
* N/M - decrease/increase the split threshold in large steps


## Benchmarks

To execute benchmarks activate the line *BinRasterizerTest.Benchmarks.runBenchmark(_argv) |> ignore* in the Program.fs.
Set a mesh in *init()* function of the *Test* module.
Currently, there are 3 parameters that are combined with each other.
* sceneView - Currently there are 3 defined scenes. 0 - captures the whole scene from a mid range, some bins are filled. 1 - is a close up view that captures a part of the scene, all bins are full. 2 - view from far away, the whole scene is a small number of bins.
* binSize - The starting bin size before the splits (if there are any). Values: 32, 64 and 128.
* maxSplits - Number of maximum splits. Values: 0, 1 and 2



## Current Findings


For fast rasterization small bins are very useful. However, sorting the triangles into the bins is significantly faster for large bins.
The quadtree structure minimally decreases the frametime due to faster rasterization. However, building the tree generates some overhead, which is why the overall benefit is relatively small.
The best improvement was the simple GPU load balancing of the rasterizer. 
Our current implementation renders a sponza scene with ~200k triangles in 50ms-70ms on a NVidia GeForce RTX 3060 Laptop GPU. However, there is still a lot of potential for many performance and memory optimizations.