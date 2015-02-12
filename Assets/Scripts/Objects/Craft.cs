using UnityEngine;
using System;
using SQLite4Unity3d;

[Table("CRAFTING")]
public class Craft {
    [PrimaryKey, AutoIncrement, Column("ID")]
    public int Id { get; set; }

    [Column("ITEM_1")]
    public string item1 { get; set; }
    [Column("ITEM_1_QUANTITY")]
    public int item1Quantity { get; set; }

    [Column("ITEM_2")]
    public string item2 { get; set; }
    [Column("ITEM_2_QUANTITY")]
    public int item2Quantity { get; set; }

    [Column("RESULT")]
    public string result { get; set; }
    [Column("RESULT_QUANTITY")]
    public int resultQuantity { get; set; }
}