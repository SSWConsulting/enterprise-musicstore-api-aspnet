using System;
using System.ComponentModel.DataAnnotations;

namespace SSW.MusicStore.Data.Entities
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        [MaxLength(256)]
        public string CartId { get; set; }
        public int AlbumId { get; set; }
        public int Count { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        public virtual Album Album { get; set; }
    }
}