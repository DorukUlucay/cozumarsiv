//based on  https://stackoverflow.com/a/6877239/1397858

//attribute
    public class ExportAttribute : Attribute
    {
        public int Order { get; set; }
        public ExportAttribute(int order)
        {
            Order = order;
        }
    }
    
    
    
//model
 public class somemodel
    {
        [Export(1)] public string asd       { get; set; }
        [Export(2)] public string sdf      { get; set; }
        [Export(3)] public string hgf   { get; set; }
        [Export(4)] public string dfg       { get; set; }
        [Export(5)] public string awd      { get; set; }
        [Export(6)] public string Street      { get; set; }
        [Export(7)] public string csdd     { get; set; }
        }
        
        
        //ordering
          PropertyInfo[] propertyList = list.Count > 0 ? list[0].GetType().GetProperties() : typeof(T).GetProperties();

          propertyList = propertyList.Select(x => new
            {
                Property = x,
                Attribute = (ExportAttribute)Attribute.GetCustomAttribute(x, typeof(ExportAttribute), true)
            }).OrderBy(x => x.Attribute != null ? x.Attribute.Order : -1).Select(x => x.Property).ToArray();
