UnityXmlLevels
==============

Load and save levels in Unity using XML files, that you can also edit manually easily.

XZ pane as using NavMesh

Example XML

```
<?xml version="1.0" encoding="us-ascii"?>

<Levels>

  <Developer StartLevel="1" />
  
  <Level>
    <Item prefab="BlueCapsule" x="1" y="2" />
    <Item prefab="RedCube" x="4" y="1" rot="45" />
    <Item prefab="RedCube" y="-2" rot="30" scale_y="3" />
    <Item prefab="GreenSphere" x="-4" y="-2" />
  </Level>

  <Level>
    <Item prefab="BlueCapsule" x="0" />
    <Item prefab="BlueCapsule" x="2" scale_y="1.2" />
    <Item prefab="BlueCapsule" x="4" scale_y="1.4" />
    <Item prefab="BlueCapsule" x="6" scale_y="1.6" />
  </Level>

</Levels>
```

1. Open MainScene: Project pane -> Assets -> _LOAD_THIS_SCENE_ -> MainScene 
2. Open and place somewhere the Xml Level Editor: Click Window -> Xml Level Editor

