module Actors.Interfaces

open OpenTK
open OpenTK.Graphics.OpenGL
open System

type IActor =
    abstract member Location : Vector3d with get, set
    abstract member Move : Vector3d -> unit
    abstract member Clone : unit -> IActor

type ITurnable =
    abstract member Pitch : float with get, set
    abstract member Yaw : float with get, set
    abstract member Roll : float with get, set

type IZoomable =
    abstract member R : float with get, set
    abstract member Zoom : factor:float -> unit

type ICameraActor =
    inherit IActor
    inherit ITurnable
    inherit IZoomable
    abstract member FOV : float with get, set
    abstract member ROV : float with get, set
    abstract member ProjectView : unit -> unit

type CameraActor (location : Vector3d) =
    interface ICameraActor with
        member this.Clone () = 
            (CameraActor (this :> IActor).Location) :> IActor
        member val Location = location with get, set
        member this.Move delta = 
            let self = this :> IActor
            //self.Location <- self.Location + delta            
            let l = Vector3d(10.,10.,10.)
            self.Location <- l

            printf "aaaaaaaaaaaaaaa%A" l
        member val Pitch = 0.0 with get, set
        member val Roll = 0.0 with get, set
        member val Yaw = 0.0 with get, set
        member val R = 1.0 with get, set
        member this.Zoom factor = 
            let self = this :> IZoomable
            self.R <- self.R * factor
        member val FOV = Math.PI * 0.5 with get, set
        member val ROV = 500.0 with get, set
        member this.ProjectView () =
            let self = this :> ICameraActor
            GL.MatrixMode MatrixMode.Projection
            
            GL.LoadIdentity()
            
            let c = 1.
            let near = 1.0
            let far = 500.0
                
            GL.Frustum (c,-c,c,-c,near,far)
            Matrix4d.CreatePerspectiveFieldOfView(self.FOV, 1., 0.2, self.ROV)
            |> ref
            |> GL.LoadMatrix
            
            GL.MatrixMode MatrixMode.Modelview 

            let focus = self.Location
            
            let yaw, pitch, roll, r = self.Yaw, self.Pitch, self.Roll, self.R
            let eye = Vector3d(cos yaw * -r * cos pitch,  cos pitch * r * sin yaw, sin pitch * r)

            Matrix4d.LookAt(focus+eye, focus, Vector3d.UnitZ)
            |> ref
            |> GL.LoadMatrix 