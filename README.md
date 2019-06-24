# ML MISIS Assigment 1 (C#)
This repository contains a finished version of the first ML course assignment at NUST MISIS.

The task was to create a generator of two spheres of randomly distributed points that have a certain percentage _N_ of 
intersection.
After that, one has to create a linear classifier out of fully connected neural layers, which should classify the points 
correctly at least in roughly _(100 - N)_% of cases.

The task of calculating an intersection of N-dimensional spheres was too hard for me at the moment, therefore spheres placement 
was done through

1. Placing spheres' centers approximately where the desired result would be.  
2. Generate points.  
3. Calculate the percentage of intersection.  
4. Adjust the center of one sphere.  
5. Repeat steps {2,3,4} and find the appropriate centers' positions with binary search.  

The solution is definitely not optimal, but it worked well enough based on the limitation that we had - less than 500 points 
for one class.

We also had to implement the backpropagation algorithm for the NN, as well as the whole NN itself from scratch.
