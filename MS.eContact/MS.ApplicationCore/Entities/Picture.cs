using MS.ApplicationCore.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MS.ApplicationCore.Entities
{
    public class Picture: BaseEntity
    {
        public Guid? PictureId { get; set; }
        public string? Description { get; set; }
        public string? UrlPath { get; set; }
        public Guid? AlbumId { get; set; }
        public int TotalViews { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string? ModifiedBy { get; set; }
        public string? UrlFullPath
        {
            get
            {
                var random = new Random();
                if (UrlPath != null)
                    return String.Format("{0}/{1}?v={2}", CommonConst.ServerFileUrl, UrlPath, random.Next(1, 999));
                return null;
            }
        }

    }
}
