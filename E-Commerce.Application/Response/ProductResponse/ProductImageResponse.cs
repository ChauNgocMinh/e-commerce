﻿namespace E_Commerce.Application.Response.ProductResponse
{
    public class ProductImageResponse
    {
        public int Id { get; set; }
        public string UrlImage { get; set; }
        public int Thunderbool { get; set; }
        public bool IsMain { get; set; }
        public Guid ProductId { get; set; }
    }
}
