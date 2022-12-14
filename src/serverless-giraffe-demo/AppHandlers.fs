module AppHandlers

open System
open System.Text.Json
open Microsoft.Extensions.Logging
open Giraffe
open Microsoft.AspNetCore.Http
open Amazon.DynamoDBv2
open Giraffe
open FSharp.AWS.DynamoDB
open FSharp.Data



let indexHandler  =
    fun (next : HttpFunc) (ctx : HttpContext) ->

        json "Serverless Giraffe Web API" next ctx

let arrayExampleHandler (itemCount:int) =
    fun (next : HttpFunc) (ctx : HttpContext) ->
        
        let values = seq { for a in 1 .. itemCount do yield sprintf "value %i" a }
        json values next ctx

let webApp:HttpHandler =
    choose [
        GET >=>
            choose [
                route "/" >=> indexHandler
                route "/array" >=> arrayExampleHandler 2
                routef "/array/%i" arrayExampleHandler
            ]
        setStatusCode 404 >=> text "Not Found" ]

// ---------------------------------
// Error handler
// ---------------------------------

let errorHandler (ex : Exception) (logger : ILogger) =
    logger.LogError(EventId(), ex, "An unhandled exception has occurred while executing the request.")
    clearResponse >=> setStatusCode 500 >=> text ex.Message

