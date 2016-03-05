using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace SSW.MusicStore.Data.Entities
{
    public class Cart
    {
        public Cart()
        {
            this.CartItems = new Collection<CartItem>();
        }

        [Required]
        [MaxLength(256)]
        public string CartId { get; set; }

        public virtual ICollection<CartItem> CartItems { get; set; }

        public int GetCount()
        {
            return this.CartItems.Select(c => c.Count).Sum();
        }

        public decimal GetTotal()
        {
            return this.CartItems.Select(c => c.Count * c.Album.Price).Sum();
        }
    }
}
