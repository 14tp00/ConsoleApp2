module WorldData
open OpenTK
open System
open LightSource
open Actors.Interfaces

type InputMode =
    | Rotate
    | Default 

type WD() = 
    static member Width = 900 
    static member Height = 900 

    static member val focus = Vector3d(0., 0., 0.) with get, set

    static member val inputMode = Default with get, set

    static member val r = 1. with get, set
    static member val yaw = Math.PI with get, set
    static member val pitch = 0. with get, set
    static member val moveV = Vector3d(0., 0., 0.) with get, set       
    static member val lights = [(PointLight(Vector3d(1.,1.,1.),1.,Color.White)):>ILightSource]
    static member Camera = CameraActor ( Vector3d(1.,1.,1.) ) :> ICameraActor