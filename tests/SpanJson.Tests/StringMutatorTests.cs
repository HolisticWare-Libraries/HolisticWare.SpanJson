using SpanJson.Internal;
using Xunit;

namespace SpanJson.Tests
{
    public class StringMutatorTests
    {
        [Fact]
        public void ToCamelCaseTest()
        {
            Assert.Equal("urlValue", StringMutator.ToCamelCase("URLValue"));
            Assert.Equal("url", StringMutator.ToCamelCase("URL"));
            Assert.Equal("id", StringMutator.ToCamelCase("ID"));
            Assert.Equal("i", StringMutator.ToCamelCase("I"));
            Assert.Equal("", StringMutator.ToCamelCase(""));
            Assert.Null(StringMutator.ToCamelCase(null));
            Assert.Equal("person", StringMutator.ToCamelCase("Person"));
            Assert.Equal("iPhone", StringMutator.ToCamelCase("iPhone"));
            Assert.Equal("iPhone", StringMutator.ToCamelCase("IPhone"));
            Assert.Equal("i Phone", StringMutator.ToCamelCase("I Phone"));
            Assert.Equal("i  Phone", StringMutator.ToCamelCase("I  Phone"));
            Assert.Equal(" IPhone", StringMutator.ToCamelCase(" IPhone"));
            Assert.Equal(" IPhone ", StringMutator.ToCamelCase(" IPhone "));
            Assert.Equal("isCIA", StringMutator.ToCamelCase("IsCIA"));
            Assert.Equal("vmQ", StringMutator.ToCamelCase("VmQ"));
            Assert.Equal("xml2Json", StringMutator.ToCamelCase("Xml2Json"));
            Assert.Equal("snAkEcAsE", StringMutator.ToCamelCase("SnAkEcAsE"));
            Assert.Equal("snA__kEcAsE", StringMutator.ToCamelCase("SnA__kEcAsE"));
            Assert.Equal("snA__ kEcAsE", StringMutator.ToCamelCase("SnA__ kEcAsE"));
            Assert.Equal("already_snake_case_ ", StringMutator.ToCamelCase("already_snake_case_ "));
            Assert.Equal("isJSONProperty", StringMutator.ToCamelCase("IsJSONProperty"));
            Assert.Equal("shoutinG_CASE", StringMutator.ToCamelCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31T23:59:59.9999999Z", StringMutator.ToCamelCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("hi!! This is text. Time to test.", StringMutator.ToCamelCase("Hi!! This is text. Time to test."));
            Assert.Equal("building", StringMutator.ToCamelCase("BUILDING"));
            Assert.Equal("building Property", StringMutator.ToCamelCase("BUILDING Property"));
            Assert.Equal("building Property", StringMutator.ToCamelCase("Building Property"));
            Assert.Equal("building PROPERTY", StringMutator.ToCamelCase("BUILDING PROPERTY"));
        }

        [Fact]
        public void ToSnakeCaseTest()
        {
            Assert.Equal("url_value", StringMutator.ToSnakeCase("URLValue"));
            Assert.Equal("url", StringMutator.ToSnakeCase("URL"));
            Assert.Equal("id", StringMutator.ToSnakeCase("ID"));
            Assert.Equal("i", StringMutator.ToSnakeCase("I"));
            Assert.Equal("", StringMutator.ToSnakeCase(""));
            Assert.Null(StringMutator.ToSnakeCase(null));
            Assert.Equal("person", StringMutator.ToSnakeCase("Person"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase("iPhone"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase("IPhone"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase("I Phone"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase("I  Phone"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase(" IPhone"));
            Assert.Equal("i_phone", StringMutator.ToSnakeCase(" IPhone "));
            Assert.Equal("is_cia", StringMutator.ToSnakeCase("IsCIA"));
            Assert.Equal("vm_q", StringMutator.ToSnakeCase("VmQ"));
            Assert.Equal("xml2_json", StringMutator.ToSnakeCase("Xml2Json"));
            Assert.Equal("sn_ak_ec_as_e", StringMutator.ToSnakeCase("SnAkEcAsE"));
            Assert.Equal("sn_a__k_ec_as_e", StringMutator.ToSnakeCase("SnA__kEcAsE"));
            Assert.Equal("sn_a__k_ec_as_e", StringMutator.ToSnakeCase("SnA__ kEcAsE"));
            Assert.Equal("already_snake_case_", StringMutator.ToSnakeCase("already_snake_case_ "));
            Assert.Equal("is_json_property", StringMutator.ToSnakeCase("IsJSONProperty"));
            Assert.Equal("shouting_case", StringMutator.ToSnakeCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31_t23:59:59.9999999_z", StringMutator.ToSnakeCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("hi!!_this_is_text._time_to_test.", StringMutator.ToSnakeCase("Hi!! This is text. Time to test."));
        }

        [Fact]
        public void ToAdaCaseTest()
        {
            Assert.Equal("Url_Value", StringMutator.ToAdaCase("URLValue"));
            Assert.Equal("Url_Value", StringMutator.ToAdaCase("urlValue"));
            Assert.Equal("Url", StringMutator.ToAdaCase("URL"));
            Assert.Equal("Url", StringMutator.ToAdaCase("url"));
            Assert.Equal("Id", StringMutator.ToAdaCase("ID"));
            Assert.Equal("I", StringMutator.ToAdaCase("I"));
            Assert.Equal("", StringMutator.ToAdaCase(""));
            Assert.Null(StringMutator.ToAdaCase(null));
            Assert.Equal("Person", StringMutator.ToAdaCase("Person"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("iPhone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("IPhone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("I Phone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("I  Phone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase(" IPhone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase(" IPhone "));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("i phone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase("i  phone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase(" iPhone"));
            Assert.Equal("I_Phone", StringMutator.ToAdaCase(" iPhone "));
            Assert.Equal("Is_Cia", StringMutator.ToAdaCase("IsCIA"));
            Assert.Equal("Vm_Q", StringMutator.ToAdaCase("VmQ"));
            Assert.Equal("Xml2_Json", StringMutator.ToAdaCase("Xml2Json"));
            Assert.Equal("Sn_Ak_Ec_As_E", StringMutator.ToAdaCase("SnAkEcAsE"));
            Assert.Equal("Sn_A__K_Ec_As_E", StringMutator.ToAdaCase("SnA__kEcAsE"));
            Assert.Equal("Sn_A__K_Ec_As_E", StringMutator.ToAdaCase("SnA__ kEcAsE"));
            Assert.Equal("Already_Ada_Case_", StringMutator.ToAdaCase("Already_Ada_Case_ "));
            Assert.Equal("Is_Json_Property", StringMutator.ToAdaCase("IsJSONProperty"));
            Assert.Equal("Shouting_Case", StringMutator.ToAdaCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31_T23:59:59.9999999_Z", StringMutator.ToAdaCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("Hi!!_This_Is_Text._Time_To_Test.", StringMutator.ToAdaCase("Hi!! This is text. Time to test."));
        }

        [Fact]
        public void ToMacroCaseTest()
        {
            Assert.Equal("URL_VALUE", StringMutator.ToMacroCase("URLValue"));
            Assert.Equal("URL_VALUE", StringMutator.ToMacroCase("urlValue"));
            Assert.Equal("URL", StringMutator.ToMacroCase("URL"));
            Assert.Equal("URL", StringMutator.ToMacroCase("url"));
            Assert.Equal("ID", StringMutator.ToMacroCase("ID"));
            Assert.Equal("I", StringMutator.ToMacroCase("I"));
            Assert.Equal("", StringMutator.ToMacroCase(""));
            Assert.Null(StringMutator.ToMacroCase(null));
            Assert.Equal("PERSON", StringMutator.ToMacroCase("Person"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("iPhone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("IPhone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("I Phone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("I  Phone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase(" IPhone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase(" IPhone "));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("i phone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase("i  phone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase(" iPhone"));
            Assert.Equal("I_PHONE", StringMutator.ToMacroCase(" iPhone "));
            Assert.Equal("IS_CIA", StringMutator.ToMacroCase("IsCIA"));
            Assert.Equal("VM_Q", StringMutator.ToMacroCase("VmQ"));
            Assert.Equal("XML2_JSON", StringMutator.ToMacroCase("Xml2Json"));
            Assert.Equal("KE_BA_BC_AS_E", StringMutator.ToMacroCase("KeBaBcAsE"));
            Assert.Equal("KE_B__A_BC_AS_E", StringMutator.ToMacroCase("KeB__aBcAsE"));
            Assert.Equal("KE_B__A_BC_AS_E", StringMutator.ToMacroCase("KeB__ aBcAsE"));
            Assert.Equal("ALREADY_MACRO_CASE_", StringMutator.ToMacroCase("Already_Macro_Case_ "));
            Assert.Equal("IS_JSON_PROPERTY", StringMutator.ToMacroCase("IsJSONProperty"));
            Assert.Equal("SHOUTING_CASE", StringMutator.ToMacroCase("SHOUTING_CASE"));
            Assert.Equal("9999-12-31_T23:59:59.9999999_Z", StringMutator.ToMacroCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("HI!!_THIS_IS_TEXT._TIME_TO_TEST.", StringMutator.ToMacroCase("Hi!! This is text. Time to test."));
        }

        [Fact]
        public void ToKebabCaseTest()
        {
            Assert.Equal("url-value", StringMutator.ToKebabCase("URLValue"));
            Assert.Equal("url", StringMutator.ToKebabCase("URL"));
            Assert.Equal("id", StringMutator.ToKebabCase("ID"));
            Assert.Equal("i", StringMutator.ToKebabCase("I"));
            Assert.Equal("", StringMutator.ToKebabCase(""));
            Assert.Null(StringMutator.ToKebabCase(null));
            Assert.Equal("person", StringMutator.ToKebabCase("Person"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase("iPhone"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase("IPhone"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase("I Phone"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase("I  Phone"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase(" IPhone"));
            Assert.Equal("i-phone", StringMutator.ToKebabCase(" IPhone "));
            Assert.Equal("is-cia", StringMutator.ToKebabCase("IsCIA"));
            Assert.Equal("vm-q", StringMutator.ToKebabCase("VmQ"));
            Assert.Equal("xml2-json", StringMutator.ToKebabCase("Xml2Json"));
            Assert.Equal("ke-ba-bc-as-e", StringMutator.ToKebabCase("KeBaBcAsE"));
            Assert.Equal("ke-b--a-bc-as-e", StringMutator.ToKebabCase("KeB--aBcAsE"));
            Assert.Equal("ke-b--a-bc-as-e", StringMutator.ToKebabCase("KeB-- aBcAsE"));
            Assert.Equal("already-kebab-case-", StringMutator.ToKebabCase("already-kebab-case- "));
            Assert.Equal("is-json-property", StringMutator.ToKebabCase("IsJSONProperty"));
            Assert.Equal("shouting-case", StringMutator.ToKebabCase("SHOUTING-CASE"));
            Assert.Equal("9999-12-31-t23:59:59.9999999-z", StringMutator.ToKebabCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("hi!!-this-is-text.-time-to-test.", StringMutator.ToKebabCase("Hi!! This is text. Time to test."));
        }

        [Fact]
        public void ToTrainCaseTest()
        {
            Assert.Equal("Url-Value", StringMutator.ToTrainCase("URLValue"));
            Assert.Equal("Url-Value", StringMutator.ToTrainCase("urlValue"));
            Assert.Equal("Url", StringMutator.ToTrainCase("URL"));
            Assert.Equal("Url", StringMutator.ToTrainCase("url"));
            Assert.Equal("Id", StringMutator.ToTrainCase("ID"));
            Assert.Equal("I", StringMutator.ToTrainCase("I"));
            Assert.Equal("", StringMutator.ToTrainCase(""));
            Assert.Null(StringMutator.ToTrainCase(null));
            Assert.Equal("Person", StringMutator.ToTrainCase("Person"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("iPhone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("IPhone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("I Phone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("I  Phone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase(" IPhone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase(" IPhone "));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("i phone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase("i  phone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase(" iPhone"));
            Assert.Equal("I-Phone", StringMutator.ToTrainCase(" iPhone "));
            Assert.Equal("Is-Cia", StringMutator.ToTrainCase("IsCIA"));
            Assert.Equal("Vm-Q", StringMutator.ToTrainCase("VmQ"));
            Assert.Equal("Xml2-Json", StringMutator.ToTrainCase("Xml2Json"));
            Assert.Equal("Ke-Ba-Bc-As-E", StringMutator.ToTrainCase("KeBaBcAsE"));
            Assert.Equal("Ke-B--A-Bc-As-E", StringMutator.ToTrainCase("KeB--aBcAsE"));
            Assert.Equal("Ke-B--A-Bc-As-E", StringMutator.ToTrainCase("KeB-- aBcAsE"));
            Assert.Equal("Already-Train-Case-", StringMutator.ToTrainCase("Already-Train-Case- "));
            Assert.Equal("Is-Json-Property", StringMutator.ToTrainCase("IsJSONProperty"));
            Assert.Equal("Shouting-Case", StringMutator.ToTrainCase("SHOUTING-CASE"));
            Assert.Equal("9999-12-31-T23:59:59.9999999-Z", StringMutator.ToTrainCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("Hi!!-This-Is-Text.-Time-To-Test.", StringMutator.ToTrainCase("Hi!! This is text. Time to test."));
        }

        [Fact]
        public void ToCobolCaseTest()
        {
            Assert.Equal("URL-VALUE", StringMutator.ToCobolCase("URLValue"));
            Assert.Equal("URL-VALUE", StringMutator.ToCobolCase("urlValue"));
            Assert.Equal("URL", StringMutator.ToCobolCase("URL"));
            Assert.Equal("URL", StringMutator.ToCobolCase("url"));
            Assert.Equal("ID", StringMutator.ToCobolCase("ID"));
            Assert.Equal("I", StringMutator.ToCobolCase("I"));
            Assert.Equal("", StringMutator.ToCobolCase(""));
            Assert.Null(StringMutator.ToCobolCase(null));
            Assert.Equal("PERSON", StringMutator.ToCobolCase("Person"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("iPhone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("IPhone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("I Phone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("I  Phone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase(" IPhone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase(" IPhone "));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("i phone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase("i  phone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase(" iPhone"));
            Assert.Equal("I-PHONE", StringMutator.ToCobolCase(" iPhone "));
            Assert.Equal("IS-CIA", StringMutator.ToCobolCase("IsCIA"));
            Assert.Equal("VM-Q", StringMutator.ToCobolCase("VmQ"));
            Assert.Equal("XML2-JSON", StringMutator.ToCobolCase("Xml2Json"));
            Assert.Equal("KE-BA-BC-AS-E", StringMutator.ToCobolCase("KeBaBcAsE"));
            Assert.Equal("KE-B--A-BC-AS-E", StringMutator.ToCobolCase("KeB--aBcAsE"));
            Assert.Equal("KE-B--A-BC-AS-E", StringMutator.ToCobolCase("KeB-- aBcAsE"));
            Assert.Equal("ALREADY-TRAIN-CASE-", StringMutator.ToCobolCase("Already-Train-Case- "));
            Assert.Equal("IS-JSON-PROPERTY", StringMutator.ToCobolCase("IsJSONProperty"));
            Assert.Equal("SHOUTING-CASE", StringMutator.ToCobolCase("SHOUTING-CASE"));
            Assert.Equal("9999-12-31-T23:59:59.9999999-Z", StringMutator.ToCobolCase("9999-12-31T23:59:59.9999999Z"));
            Assert.Equal("HI!!-THIS-IS-TEXT.-TIME-TO-TEST.", StringMutator.ToCobolCase("Hi!! This is text. Time to test."));
        }
    }
}