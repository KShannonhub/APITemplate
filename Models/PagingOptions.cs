﻿using System.ComponentModel.DataAnnotations;

namespace APITemplate.Models
{
    public class PagingOptions
    {
        [Range(1, 99999, ErrorMessage = "Offset must be greater than 0.")]
        public int? Limit { get; set; }
        
        [Range(1, 100, ErrorMessage = "Limit must be greater than 0 and less than 100.")]
        public int? Offset { get; set; }

        public PagingOptions Replace(PagingOptions newer)
        {
            return new PagingOptions
            {
                Limit = newer.Limit ?? Limit,
                Offset = newer.Offset ?? Offset
            };
        }
    }
}
