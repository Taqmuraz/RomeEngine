namespace RomeEngine
{
    public enum PhysicalShapeType : int
    {
        Box = 1,
        Sphere = 2,
        Mesh = 4,

        BoxVsSphere = (Box << 16) | Sphere,
        BoxVsMesh = (Box << 16) | Mesh,
        BoxVsBox = (Box << 16) | Box,

        SphereVsBox = (Sphere << 16) | Box,
        SphereVsMesh = (Sphere << 16) | Mesh,
        SphereVsSphere = (Sphere << 16) | Sphere,

        MeshVsBox = (Mesh << 16) | Box,
        MeshVsSphere = (Mesh << 16) | Sphere,
        MeshVsMesh = (Mesh << 16) | Mesh,
    }
}