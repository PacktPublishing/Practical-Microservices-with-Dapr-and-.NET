using System;

namespace sample.microservice.common
{
    public class Topics
    {
        public const string OrderSubmittedTopicName = "OnOrder_Submitted";
        public const string OrderPreparedTopicName = "OnOrder_Prepared";
        public const string OrderReadyTopicName = "OnOrder_Ready";
        public const string OrderShippedTopicName = "OnOrder_Shipped";
        public const string ReservationCompletedTopicName = "OnReservation_Completed";
        public const string ReservationFailedTopicName = "OnReservation_Failed";
        public const string CustomizationCompletedTopicName = "OnCustomization_Completed";
        public const string CustomizationFailedTopicName = "OnCustomization_Failed";
        public const string FullfillmentCompledTopicName = "OnFullfillment_Completed";
        public const string FullfillmentFailedTopicName = "OnFullfillment_Failed";
    }
}
