﻿using System;
using System.Runtime.Serialization;

namespace DataLayer.Exceptions
{
    [Serializable]
    public class DuplicatePhoneNumberException : Exception
    {
        public DuplicatePhoneNumberException() : base("The phone number entered is duplicate...") { }
    }

    [Serializable]
    public class NotFoundException : Exception
    {
        public NotFoundException() : base("Your request does not exist...") { }
    }

    [Serializable]
    public class DuplicateTransactionException : Exception
    {
        public DuplicateTransactionException() : base("The Transaction entered is duplicate...") { }
    }

    public class DoNotExistBankTransactionException : Exception
    {
        public DoNotExistBankTransactionException() : base("This transaction is not available in bank transactions...") { }
    }
}