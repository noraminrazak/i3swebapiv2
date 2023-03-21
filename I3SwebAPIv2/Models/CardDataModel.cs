using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace I3SwebAPIv2.Models
{
    public class CardDataModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }

    public class CardSearchNameParam
    {
        public int user_role_id { get; set; }
        public int school_id { get; set; }
        public string search_name { get; set; }
    }

    public class CardHashParam
    {
        public string card_pin { get; set; }
    }

    public class UpdateCardParam
    {
        public int card_id { get; set; }
        public int school_id { get; set; }
        public string update_by { get; set; }
    }

    public class UpdateCardLimitParam
    {
        public int card_id { get; set; }
        public int school_id { get; set; }
        public decimal daily_limit { get; set; }
        public string update_by { get; set; }
    }

    public class CardLimitParam
    {
        public int card_id { get; set; }
    }

    public class CardSearchStatusParam
    {
        public int card_status_id { get; set; }
        public string card_number { get; set; }
    }

    public class CardReplacementParam
    {
        public int old_card_id { get; set; }
        public int old_card_status_id { get; set; }
        public string new_card_number { get; set; }
        public int user_role_id { get; set; }
        public int school_id { get; set; }
        public string create_by { get; set; }
    }

    public class CardModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<Card> Data { get; set; }
    }

    public class Card
    {
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
    }

    public class CardSearchModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardSearch> Data { get; set; }
    }

    public class CardSearch
    {
        public int profile_id { get; set; }
        public string full_name { get; set; }
        public string photo_url { get; set; }
        public string school_name { get; set; }
        public string class_name { get; set; }
        public int card_id { get; set; }
        public string card_number { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }

    }

    public class CardDailyLimitModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardDailyLimit> Data { get; set; }
    }

    public class CardListModel
    {
        public bool Success { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public List<CardList> Data { get; set; }
    }

    public class CardDailyLimit
    {
        public int card_id { get; set; }
        public string card_number { get; set; }
        public decimal daily_limit { get; set; }
        public int card_status_id { get; set; }
        public string card_status { get; set; }
    }
}