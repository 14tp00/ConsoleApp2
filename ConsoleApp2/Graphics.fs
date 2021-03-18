module Graphics
open OpenTK
open OpenTK.Graphics.OpenGL
open Face

type GFX() =
    static member DrawLine (a:Vector3d) (b:Vector3d) (color:Color) (width:float) =
        GL.LineWidth (float32 width)
        GL.Begin PrimitiveType.Lines
        GL.Color3 color

        GL.Vertex3 a 
        GL.Vertex3 b
        GL.End()

    static member FillPolygon (points:Vector3d list) (color:Color) =
        GL.PolygonMode (MaterialFace.Front, PolygonMode.Fill)
        GL.CullFace CullFaceMode.Front 
        GL.Enable EnableCap.CullFace
        GL.Enable EnableCap.DepthTest
        GL.Begin PrimitiveType.TriangleFan
        GL.Color3 color

        List.iter (fun (p:Vector3d) -> GL.Vertex3 p) points

        GL.End()

    static member DrawCube (m:Vector3d) (s:float) (c:Color)=
        
        let x, y, z = 
            Vector3d(-s/2.,0.,0.),
            Vector3d(0.,-s/2.,0.),
            Vector3d(0.,0.,-s/2.)

        let v000, v001, v010, v100, v110, v011, v101, v111 = 
           m+x+y+z,
           m+x+y-z,
           m+x-y+z,
           m-x+y+z,
           m-x-y+z,
           m+x-y-z,
           m-x+y-z,
           m-x-y-z

        let fs = [Face(v000,v011,v001, c); Face(v010,v011,v000, c); Face(v100,v101,v111, c); Face(v100,v111,v110, c);
            Face(v100,v001,v101, c); Face(v100,v000,v001, c); Face(v111,v011,v110, c); Face(v011,v010,v110, c);
            Face(v000,v110,v010, c); Face(v000,v100,v110, c); Face(v011,v111,v001, c); Face(v111,v101,v001, c)]

        List.iter (fun (f:Face) -> f.Draw) fs


    static member DrawCubeNoShading (m:Vector3d) (s:float) (c:Color) =     
           let x, y, z = 
               Vector3d(-s/2.,0.,0.),
               Vector3d(0.,-s/2.,0.),
               Vector3d(0.,0.,-s/2.)

           let v000, v001, v010, v100, v110, v011, v101, v111 = 
              m+x+y+z,
              m+x+y-z,
              m+x-y+z,
              m-x+y+z,
              m-x-y+z,
              m+x-y-z,
              m-x+y-z,
              m-x-y-z

           let fs = [Face(v000,v011,v001, c); Face(v010,v011,v000, c); Face(v100,v101,v111, c); Face(v100,v111,v110, c);
               Face(v100,v001,v101, c); Face(v100,v000,v001, c); Face(v111,v011,v110, c); Face(v011,v010,v110, c);
               Face(v000,v110,v010, c); Face(v000,v100,v110, c); Face(v011,v111,v001, c); Face(v111,v101,v001, c)]
            
           List.iter (fun (f:Face) -> f.DrawUnshaded) fs

    static member DrawPlane (normal:Vector3d) (anchor:Vector3d) (c:Color)=
        
        let v1 = Vector3d(normal.Z, -normal.X, normal.Y)

        let x = Vector3d.Cross(normal, v1)

        let y = Vector3d.Cross(normal, x)          

        let v00, v01, v10, v11 = 
            anchor+x+y,
            anchor+x-y,
            anchor-x+y,
            anchor-x-y
               
        let fs = [Face(v00,v11,v01,c);
            Face(v00,v11,v01,c).Flipped;
            Face(v00,v10,v11,c);
            Face(v00,v10,v11,c).Flipped]

        List.iter (fun (f:Face) -> f.Draw) fs