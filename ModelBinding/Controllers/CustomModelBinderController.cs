using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ModelBinding.CustomModelBinder;
using ModelBinding.Models;

namespace ModelBinding.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomModelBinderController : ControllerBase
    {
        //Limited to Built-in Sources
        //Limitation:
        //The model binder can’t bind CustomObject directly from a custom string format like "Name:Age:Location".
        [HttpGet("custom-object-binding")]
        public IActionResult CustomObjectBinding(CustomObject complexData)
        {
            return Ok(complexData);
        }
        //Lacks Flexibility
        //Limitation:
        //The default model binder cannot easily handle merging data from multiple sources(headers, query, and body) into a single model.
        [HttpPost("multi-source-binding")]
        public IActionResult MultiSourceBinding([ModelBinder(BinderType = typeof(MultiSourceModelBinder))] MergedModel model)
        {
            // The action method returns an HTTP 200 OK response with the bound 'MergedModel' as the response body.
            // The 'MergedModel' object is populated using the custom binding logic defined in the 'MultiSourceModelBinder'.
            return Ok(model);
        }
        //No Support for Special Data Types
        //Limitation:
        //Tuples cannot be easily bound by the default model binder without additional configuration or custom binding logic.
        [HttpPost("tuple-binding")]
        //The [ModelBinder] attribute with the BinderType property specifies that a custom model binder should be used instead of the default model binder.
        //In this case, BinderType = typeof(TupleModelBinder) indicates that the TupleModelBinder class should be used to bind the incoming request data to the Tuple<int, string> parameter(tupleData).
        public IActionResult TupleBinding([ModelBinder(BinderType = typeof(TupleModelBinder))] Tuple<int, string> tupleData)
        {
            return Ok($"Tuple Data: Item1 = {tupleData.Item1}, Item2 = {tupleData.Item2}");
        }
        //Performance Issues for Large Data
        //Limitation:
        //For large payloads, FromBody reads the entire request body into memory, which can be inefficient and lead to performance issues.
        [HttpPost("large-data-binding")]
        public IActionResult LargeDataBinding([ModelBinder(BinderType = typeof(LargeDataModelBinder))] LargeDataModel model)
        {
            return Ok("Data received successfully");
        }
    }
}