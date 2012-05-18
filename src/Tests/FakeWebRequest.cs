using System;
using CheapSkate;

namespace Tests
{
    public class FakeWebRequest : IWebRequest
    {
        private readonly bool _error;
        private readonly bool _blowUp;
        private readonly string _ipAddress;

        public FakeWebRequest(bool error = false, bool blowUp = false, string ipAddress = "207.97.227.245")
        {
            _error = error;
            _blowUp = blowUp;
            _ipAddress = ipAddress;
        }

        public string Get(string url, params object[] args)
        {
            if (_blowUp) throw new Exception();
            if (!_error) return @"<?xml version=""1.0""?>
<interface-response>
    <Command>SETDNSHOST</Command>
    <Language>eng</Language>
    <IP>" + _ipAddress + @"</IP>
    <ErrCount>0</ErrCount>
    <ResponseCount>0</ResponseCount>
    <Done>true</Done>
    <debug><![CDATA[]]></debug>
</interface-response>";
            return @"<?xml version=""1.0""?>
<interface-response>
    <Command>SETDNSHOST</Command>
    <Language>eng</Language>
    <ErrCount>2</ErrCount>
    <errors>
        <Err1>Domain name not found</Err1>
        <Err2>Invalid key</Err2>
    </errors>
    <ResponseCount>1</ResponseCount>
    <responses>
        <response>
            <ResponseNumber>316153</ResponseNumber>
            <ResponseString>Validation error; not found; domain name(s)</ResponseString>
        </response>
    </responses>
    <Done>true</Done>
    <debug><![CDATA[]]></debug>
</interface-response>";
        }
    }
}