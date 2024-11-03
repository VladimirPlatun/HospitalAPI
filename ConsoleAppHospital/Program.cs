using System;
using ConsoleAppHospital.Service;
using HospitalAPI.Model;

namespace ConsoleAppHospital 
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            await PatientGenerator.GenerateAndSendPatients();

        }
    }
}