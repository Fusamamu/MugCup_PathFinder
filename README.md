# MugCup PathFinder For Unity
#### Created by Sukum Duangpattra [MUGCUP], using Unity [version 2020.3.22.f1]

## Introduction

<p>
This is a PathFinder Tool Package for Unity using A* Star algorithm. The Package also contains Grid Utility
class to help generate and manage a grid of nodes.
</p>

## Features
<ul>
    <li>"Grid Utility Class", this provide useful methods to create a grid of nodes for path finder.</li>
    <li>"Static Helper Class" provided to calculate a path on a fly.</li>
    <li>A PathFinder Component attached direcly to GameObject so that it can find a path individually.</li>
    <li>A PathFinder Controller used to make requests for paths using Async.</li>
</ul>

## Installation Instruction
<p>

</p>

## Grid Data Setting
<p>
To be able to use PathFinder, a grid of nodes, containing position and cost among other thing, must be created.
Script "GridNodeData" is used for this purpose. 
</p>

### How to generate a grid of nodes



## Examples
### Use Static Helper Class

```csharp
    public IEnumerable<NodeBase> GetPathUsingStaticPathFinder()
    {
        AStarPathFinder<NodeBase>.InitializeGridData(gridSize, gridNodes);
            
        var _path = AStarPathFinder<NodeBase>.FindPath(gridNodes[0], gridNodes[40]).ToArray();

        return _path;
    }
```

### Use "PathFinder.cs" Component
<p>
To use "PathFinder.cs", Create a gameObjec you want it to move along a path and attach the script directly to it.
You will be able to make a customed configuration to your own desire.
</p>

<p>

>The Package comes with nice custom editor for PathFinder Component.

</p>

### Use "PathFinderController.cs"
<p>
If you 
</p>


## License

<p>
This library is under the MIT License.
</p>


### TODO

- [ ] Resolve Retracing Path Method.
