# MugCup PathFinder For Unity
#### Created by Sukum Duangpattra [MUGCUP], using Unity [version 2020.3.22.f1]

## Introduction

<p>
This is a PathFinder Tool Package for Unity using A* Star algorithm. The Package also contains Grid Utility
class to help generate and manage a grid of nodes.
</p>

## Features
<ul>
    <li>"Static Helper Class" provided to calculate a path on a fly.</li>
    <li>A PathFinder Component attached direcly to GameObject so that it can find a path individually.</li>
    <li>A PathFinder Controller used to make requests for paths using Async.</li>
</ul>

## Installation Instruction
<p>

</p>

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

<p>

>The Package comes with nice custom editor for PathFinder Component.

</p>

## License

<p>
This library is under the MIT License.
</p>


### TODO

- [ ] Resolve Retracing Path Method.
