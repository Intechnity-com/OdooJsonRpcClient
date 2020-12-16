using System;
using Newtonsoft.Json;
using PortaCapena.OdooJsonRpcClient.Attributes;
using PortaCapena.OdooJsonRpcClient.Converters;
using PortaCapena.OdooJsonRpcClient.Models;

namespace PortaCapena.OdooJsonRpcClient.Shared.Models
{
    [OdooTableName("stock.picking.type")]
    [JsonConverter(typeof(OdooModelConverter))]
    public class StockPickingTypeOdooDto : IOdooModel
    {

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("color")]
        public int? Color { get; set; }

        [JsonProperty("sequence")]
        public int? Sequence { get; set; }

        // ir.sequence
        [JsonProperty("sequence_id")]
        public long? SequenceId { get; set; }

        [JsonProperty("sequence_code")]
        public string SequenceCode { get; set; }

        // stock.location
        [JsonProperty("default_location_src_id")]
        public long? DefaultLocationSrcId { get; set; }

        // stock.location
        [JsonProperty("default_location_dest_id")]
        public long? DefaultLocationDestId { get; set; }

        [JsonProperty("code")]
        public string Code { get; set; }

        // stock.picking.type
        [JsonProperty("return_picking_type_id")]
        public long? ReturnPickingTypeId { get; set; }

        [JsonProperty("show_entire_packs")]
        public bool? ShowEntirePacks { get; set; }

        // stock.warehouse
        [JsonProperty("warehouse_id")]
        public long? WarehouseId { get; set; }

        [JsonProperty("active")]
        public bool? Active { get; set; }

        [JsonProperty("use_create_lots")]
        public bool? UseCreateLots { get; set; }

        [JsonProperty("use_existing_lots")]
        public bool? UseExistingLots { get; set; }

        [JsonProperty("show_operations")]
        public bool? ShowOperations { get; set; }

        [JsonProperty("show_reserved")]
        public bool? ShowReserved { get; set; }

        [JsonProperty("count_picking_draft")]
        public int? CountPickingDraft { get; set; }

        [JsonProperty("count_picking_ready")]
        public int? CountPickingReady { get; set; }

        [JsonProperty("count_picking")]
        public int? CountPicking { get; set; }

        [JsonProperty("count_picking_waiting")]
        public int? CountPickingWaiting { get; set; }

        [JsonProperty("count_picking_late")]
        public int? CountPickingLate { get; set; }

        [JsonProperty("count_picking_backorders")]
        public int? CountPickingBackorders { get; set; }

        [JsonProperty("rate_picking_late")]
        public int? RatePickingLate { get; set; }

        [JsonProperty("rate_picking_backorders")]
        public int? RatePickingBackorders { get; set; }

        [JsonProperty("barcode")]
        public string Barcode { get; set; }

        // res.company
        [JsonProperty("company_id")]
        public long CompanyId { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("display_name")]
        public string DisplayName { get; set; }

        // res.users
        [JsonProperty("create_uid")]
        public long? CreateUid { get; set; }

        [JsonProperty("create_date")]
        public DateTime? CreateDate { get; set; }

        // res.users
        [JsonProperty("write_uid")]
        public long? WriteUid { get; set; }

        [JsonProperty("write_date")]
        public DateTime? WriteDate { get; set; }

        [JsonProperty("__last_update")]
        public DateTime? LastUpdate { get; set; }
    }


}