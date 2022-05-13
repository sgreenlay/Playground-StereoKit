using StereoKit;
using System.Numerics;

namespace Playground;

public class App
{
    public SKSettings Settings => new SKSettings
    {
        appName = "StereoKit Template",
        assetsFolder = "Assets",
        displayPreference = DisplayMode.MixedReality
    };

    Pose cubePose = new Pose(0, 0, -0.5f, Quat.Identity);
    Model cube;
    Matrix4x4 floorTransform = Matrix.TS(new Vector3(0, -1.5f, 0), new Vector3(30, 0.1f, 30));
    Material floorMaterial;

    public void Init()
    {
        // Create assets used by the app
        cube = Model.FromMesh(
            Mesh.GenerateCube(Vec3.One * 0.1f),
            Default.MaterialUI);

        floorMaterial = new Material(Shader.FromFile("floor.hlsl"));
        floorMaterial.Transparency = Transparency.Blend;
    }

    public void Step()
    {
        if (SK.System.displayType == Display.Opaque)
            Default.MeshCube.Draw(floorMaterial, floorTransform);

        UI.Handle("Cube", ref cubePose, cube.Bounds);
        cube.Draw(cubePose.ToMatrix());
    }
}
