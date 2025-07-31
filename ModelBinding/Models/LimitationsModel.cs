using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.CustomModelBinder;

namespace ModelBinding.Models
{
    //This Model will use to demonstrate Limited to Built-in Sources
    // The [ModelBinder] attribute is used to specify the custom model binder for this class.
    // The BinderType parameter is set to typeof(CustomObjectModelBinder), indicating that the CustomObjectModelBinder
    // should be used to bind incoming request data to instances of the CustomObject class.
    [ModelBinder(BinderType = typeof(CustomObjectModelBinder))]
    public class CustomObject
    {
        // This property will store the 'Name' part extracted by the custom model binder
        public string Name { get; set; }
        // This property will store the 'Age' part extracted and parsed by the custom model binder
        public int Age { get; set; }
        // This property will store the 'Location' part extracted by the custom model binder
        public string Location { get; set; }
    }
    
    //This Model will use to demonstrate Lacks Flexibility
    public class ComplexBodyModel
    {
        public string Data { get; set; }
    }
    //This Model will use to demonstrate Lacks Flexibility
    public class MergedModel
    {
        public string Header { get; set; }
        public string Query { get; set; }
        public string BodyData { get; set; }
    }
    //This Model will use to demonstrate No Support for Special Data Types
    public class CustomTupleModel
    {
        public int Item1 { get; set; }
        public string Item2 { get; set; }
    }
    //This Model will use to demonstrate Performance Issues for Large Data
    public class LargeDataModel
    {
        public List<string> LargeDataList { get; set; }
    }
}