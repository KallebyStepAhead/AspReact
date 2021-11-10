using System.Collections.Generic;

namespace ViteReact.Shared.DTO
{
    public class ListReadDTO<TReadDTO>
    {
        public int Total { get; set; }

        public IEnumerable<TReadDTO> Data { get; set; }
    }
}