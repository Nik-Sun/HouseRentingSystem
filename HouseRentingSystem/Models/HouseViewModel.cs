﻿using System.ComponentModel;

namespace HouseRentingSystem.Models
{
    public class HouseViewModel
    {
        public int Id { get; init; }

        public string Title { get; init; } = null!;

        public string Address { get; init; } = null!;

        [DisplayName("Image URL")]
        public string ImageUrl { get; init; } = null!;
    }
}