using Core.Entities;
using System;

namespace Entities.Dtos
{
    public class UserPortfolioDto : IDto
    {
        public Guid Id { get; set; }
        public int UserId { get; set; }
        public Guid ShareId { get; set; }
        public int Lot { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedUser { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? UpdatedUser { get; set; }
    }
}
