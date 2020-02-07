using System;

namespace Data.Schema
{
   public class OssIndexModel
   {
       public int Id { get; set; }
       public int ComponentId { get; set; }
       public Decimal Version { get; set; }
       public string TypeFormat { get; set; }
       public string Coordinates { get; set; }
       public string Description { get; set; }
       public string Reference { get; set; }
       public DateTime ExpireDate { get; set; }
       public int HttpStatus { get; set; }
   }
}
