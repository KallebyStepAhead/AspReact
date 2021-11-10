using System.ComponentModel.DataAnnotations;

namespace ViteReact.Shared.DTO
{
    public class PaginateDTO
    {
        [Range(0, int.MaxValue)]
        public int Skip { get; set; } = 0;

        [Range(0, int.MaxValue)]
        public int Limit { get; set; } = 0;
    }
}