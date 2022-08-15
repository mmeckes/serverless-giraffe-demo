module Types

open System

type Product = {
    id: Guid
    name: string
    price: decimal
}

type Products = {
    products: Product[]
}

