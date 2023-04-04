using System;
using FluentAssertions;
using System.Linq;
using PortaCapena.OdooJsonRpcClient.Models;
using PortaCapena.OdooJsonRpcClient.Shared.Models;
using Xunit;
using PortaCapena.OdooJsonRpcClient.Converters;
using Newtonsoft.Json;
using Xunit.Abstractions;
using System.IO;

namespace PortaCapena.OdooJsonRpcClient.Tests
{

    public class OdooModelMapperTests
    {
        [Fact]
        public void CanMap_StringToEnum()
        {
            object output;

            /*
             *  In StatusPurchaseOrderOdooEnum we have this declaration :
             *  
             *  [EnumMember(Value = "done")]
             *  Locked = 5,
             *  
             *  So the JValue should be "done" (String) and the output should be Locked
             * */

            OdooModelMapper.ConverOdooPropertyToDotNet(
                typeof(StatusPurchaseOrderOdooEnum),
                new Newtonsoft.Json.Linq.JValue("done"),
                out output
            );

            var result = (StatusPurchaseOrderOdooEnum)output;

            Assert.Equal(StatusPurchaseOrderOdooEnum.Locked, result);
        }


        [Fact]
        public void CanMap_IntegerToEnum()
        {
            object output;

            /*
             *  In PriorityPurchaseOrderOdooEnum we have this declaration :
             *  
             *  [EnumMember(Value = "1")]
             *  Urgent = 2,
             *  
             *  So the JValue should be 1 (Integer) and the output should be Urgent
             * */

            OdooModelMapper.ConverOdooPropertyToDotNet(
                typeof(PriorityPurchaseOrderOdooEnum),
                new Newtonsoft.Json.Linq.JValue(1),
                out output
            );

            var result = (PriorityPurchaseOrderOdooEnum)output;

            Assert.Equal(PriorityPurchaseOrderOdooEnum.Urgent, result);
        }
    }
}