using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using FluentAssertions;
using Nomination.BusinessLayer.Interfaces;
using Nomination.BusinessLayer.Services.ConfirmationResponse;
using Nomination.BusinessLayer.Services.Naesb;
using Nomination.BusinessLayer.Services.RequestForConfirmation;
using Nomination.BusinessLayer.Services.ScheduledQuantity;
using Nomination.Domain.RequestForConfirmation;
using Xunit;
using Location = Nomination.Domain.RequestForConfirmation.Location;
using Stream = Nomination.Domain.RequestForConfirmation.Stream;

namespace SchedulingEngineUnitTest
{
    public class SchedulingEngineUnitTest
    {
        public RequestForConfirmation MockRfc =
            new RequestForConfirmation
            {
                PurchaseOrderNumber = "PGL_ANR_20181114TIM",
                GasDayStart = "201811140900",
                GasDayEnd = "201811150900",
                GasDay = new DateTime(2018, 11, 14, 00, 00, 00),
                Cycle = "TIM",
                PartyIndentificaton = new PartyIndentificaton
                {
                    PipelineEntity = "006958581",
                    UtilityEntity = "006932115"
                },
                Locations = new List<Location>()
                {
                    new Location
                    {
                        Id = "3",
                        ContractNominations = new List<ContractNomination>
                        {
                            new ContractNomination
                            {
                                Id = "PGL-Nominations",
                                Nominations = new List<Nomination.Domain.RequestForConfirmation.Nomination>
                                {
                                    new Nomination.Domain.RequestForConfirmation.Nomination
                                    {
                                        Id = "15016830",
                                        Quantity = "16791",
                                        FlowIndicator = "R",
                                        Stream = new Stream
                                        {
                                            Direction = "UP",
                                            ContractId = "131176"
                                        },
                                        NomsContractInfo = new NomsContractInfo
                                        {
                                            EntityId = "800194"
                                        }
                                    },
                                    new Nomination.Domain.RequestForConfirmation.Nomination
                                    {
                                        Id = "15016846",
                                        Quantity = "800",
                                        FlowIndicator = "R",
                                        Stream = new Stream
                                        {
                                            Direction = "UP",
                                            ContractId = "110506"
                                        },
                                        NomsContractInfo = new NomsContractInfo
                                        {
                                            EntityId = "825004"
                                        }
                                    }
                                }

                            }
                        }
                    },
                    new Location
                    {
                        Id = "4"
                    }
                }
            };


        [Fact]
        public void TestRequestForConfirmation()
        {
        //    ///////////////////////////
        //    //arrange - mock it up
        //    ///////////////////////////
        //    DateTime gasDay = new DateTime(2018, 11, 14, 00, 00, 00);

        //    RequestForConfirmation mockRfc =
        //        new RequestForConfirmation
        //        {
        //            PurchaseOrderNumber = "PGL_ANR_20181114TIM",
        //            GasDayStart = "201811140900",
        //            GasDayEnd = "201811150900",
        //            GasDay = gasDay,
        //            Cycle = "TIM",
        //            PartyIndentificaton = new PartyIndentificaton
        //            {
        //                PipelineEntityId = "006958581",
        //                UtilityEntityId = "006932115"
        //            },
        //            Locations = new List<Location>()
        //            {
        //                new Location
        //                {
        //                    Id = "3",
        //                    ContractNominations = new List<ContractNomination>
        //                    {
        //                        new ContractNomination
        //                        {
        //                            Id = "PGL-Nominations",
        //                            Nominations = new List<Nomination.Domain.RequestForConfirmation.Nomination>
        //                            {
        //                                new Nomination.Domain.RequestForConfirmation.Nomination
        //                                {
        //                                    Id = "15016830",
        //                                    Quantity = "16791",
        //                                    FlowIndicator = "R",
        //                                    Stream = new Stream
        //                                    {
        //                                        Direction = "UP",
        //                                        ContractId = "131176"
        //                                    },
        //                                    NomsContractInfo = new NomsContractInfo
        //                                    {
        //                                        EntityId = "800194"
        //                                    }
        //                                },
        //                                new Nomination.Domain.RequestForConfirmation.Nomination
        //                                {
        //                                    Id = "15016846",
        //                                    Quantity = "800",
        //                                    FlowIndicator = "R",
        //                                    Stream = new Stream
        //                                    {
        //                                        Direction = "UP",
        //                                        ContractId = "110506"
        //                                    },
        //                                    NomsContractInfo = new NomsContractInfo
        //                                    {
        //                                        EntityId = "825004"
        //                                    }
        //                                }
        //                            }

        //                        }
        //                    }
        //                },
        //                new Location
        //                {
        //                    Id = "4"
        //                }
        //            }
        //        };

        //    Mock<IRequestForConfirmationRepository> moqRequestForConfirmationRepository = new Mock<IRequestForConfirmationRepository>();
        //    moqRequestForConfirmationRepository
        //        .Setup(x => x.Get("ANR", "PGL", gasDay, "TIM"))
        //        .Returns(mockRfc);

        //    RequestForConfirmationGet serviceRequestForConfirmationGet = new RequestForConfirmationGet(moqRequestForConfirmationRepository.Object);

        //    /////////
        //    //act
        //    /////////
        //    RequestForConfirmation serviceRfc = serviceRequestForConfirmationGet.Invoke("ANR", "PGL", gasDay, "TIM");

        //    /////////
        //    //assert
        //    /////////
        //    Assert.NotNull(serviceRfc);
        //    moqRequestForConfirmationRepository.Verify(
        //        i => i.Get(
        //              "ANR"
        //            , "PGL"
        //            , gasDay
        //            , "TIM")
        //    );

        //    Assert.Equal("PGL_ANR_20181114TIM", serviceRfc.PurchaseOrderNumber);
        //    Assert.Equal("201811140900", serviceRfc.GasDayStart);
        //    Assert.Equal("201811150900", serviceRfc.GasDayEnd);
        //    Assert.Equal(gasDay, serviceRfc.GasDay);
        //    Assert.Equal("TIM", serviceRfc.Cycle);

        //    serviceRfc.PartyIndentificaton
        //        .Should()
        //        .BeEquivalentTo(
        //            new PartyIndentificaton
        //            {
        //                PipelineEntityId = "006958581",
        //                UtilityEntityId = "006932115"
        //            });

        //    serviceRfc.Locations
        //        .Should()
        //        .BeEquivalentTo(
        //            new List<Location>()
        //            {
        //                new Location
        //                {
        //                    Id = "3",
        //                    ContractNominations = new List<ContractNomination>
        //                    {
        //                        new ContractNomination
        //                        {
        //                            Id = "PGL-Nominations",
        //                            Nominations = new List<Nomination.Domain.RequestForConfirmation.Nomination>
        //                            {
        //                                new Nomination.Domain.RequestForConfirmation.Nomination
        //                                {
        //                                    Id = "15016830",
        //                                    Quantity = "16791",
        //                                    FlowIndicator = "R",
        //                                    Stream = new Stream
        //                                    {
        //                                        Direction = "UP",
        //                                        ContractId = "131176"
        //                                    },
        //                                    NomsContractInfo = new NomsContractInfo
        //                                    {
        //                                        EntityId = "800194"
        //                                    }
        //                                },
        //                                new Nomination.Domain.RequestForConfirmation.Nomination
        //                                {
        //                                    Id = "15016846",
        //                                    Quantity = "800",
        //                                    FlowIndicator = "R",
        //                                    Stream = new Stream
        //                                    {
        //                                        Direction = "UP",
        //                                        ContractId = "110506"
        //                                    },
        //                                    NomsContractInfo = new NomsContractInfo
        //                                    {
        //                                        EntityId = "825004"
        //                                    }
        //                                }
        //                            }

        //                        }
        //                    }
        //                },
        //                new Location
        //                {
        //                    Id = "4"
        //                }
        //            }
        //        );

        }

        [Fact]
        public void TestProcessRequestForConfirmation()
        {
            ///////////////////////////////
            //////arrange - mock it up
            ///////////////////////////////
            ////DateTime gasDay = new DateTime(2018, 11, 14, 00, 00, 00);

            ////Mock<INaesbEventProcessRepository> moqNaesbEventProcessRepository = new Mock<INaesbEventProcessRepository>();
            ////moqNaesbEventProcessRepository
            ////    .Setup(x => x.Create(It.IsAny<Nomination.Domain.Naesb.NaesbEventProcess>()))
            ////    .Returns(999);


            ////Mock<IRequestForConfirmationRepository> moqRequestForConfirmationRepository = new Mock<IRequestForConfirmationRepository>();
            ////moqRequestForConfirmationRepository
            ////    .Setup(x => x.Get("ANR", "PGL", gasDay, "TIM"))
            ////    .Returns(MockRfc);
            ////RequestForConfirmationGet serviceRequestForConfirmationGet = new RequestForConfirmationGet(moqRequestForConfirmationRepository.Object);

            
            ////moqRequestForConfirmationRepository
            ////    .Setup(x => x.Create(
            ////        It.IsAny<Nomination.Domain.RequestForConfirmation.RequestForConfirmation>(),
            ////        "xxxxx"
            ////    ));
            ////RequestForConfirmationCreate serviceRequestForConfirmationCreate = new RequestForConfirmationCreate(
            ////    new Nomination.BusinessLayer.Services.ServiceSettings { UserId = "xxxxx" },
            ////    moqRequestForConfirmationRepository.Object
            ////);


            ////Mock<INaesbPipelineRepository> moqNaesbPipelineRepository = new Mock<INaesbPipelineRepository>();
            ////moqNaesbPipelineRepository
            ////    .Setup(x => x.GetByPipelineEntityID("006958581"))
            ////    .Returns(new Nomination.Domain.Naesb.NaesbPipeline
            ////    {
            ////        Pipeline = "ANR",
            ////        PipelineEntityId = "006958581"

            ////    });
            ////NaesbPipelineGetByEntityId serviceNaesbPipeline = new NaesbPipelineGetByEntityId(moqNaesbPipelineRepository.Object);

            ////Mock<INaesbUtilityRepository> moqNaesbUtilityRepository = new Mock<INaesbUtilityRepository>();
            ////moqNaesbUtilityRepository
            ////    .Setup(x => x.GetByUtilityEntityId("006932115"))
            ////    .Returns(new Nomination.Domain.Naesb.NaesbUtility
            ////    {
            ////        Utility = "PGL",
            ////        UtilityEntityId = "006932115"
            ////    });
            ////NaesbUtilityGetByEntityId serviceNaesbUtility = new NaesbUtilityGetByEntityId(moqNaesbUtilityRepository.Object);


            ////NaesbEventProcessCreateRfc serviceNaesbEventProcessCreateRfc = new NaesbEventProcessCreateRfc(
            ////    new Nomination.BusinessLayer.Services.ServiceSettings { UserId = "xxxxx" },
            ////    moqNaesbEventProcessRepository.Object,
            ////    serviceRequestForConfirmationGet,
            ////    serviceRequestForConfirmationCreate,
            ////    null,
            ////    serviceNaesbPipeline,
            ////    serviceNaesbUtility
            ////);

            /////////////
            //////act
            /////////////
            ////int id = serviceNaesbEventProcessCreateRfc.Invoke(DateTime.Now, gasDay, "ANR", "PGL", "TIM");

            /////////////
            //////assert
            /////////////
            ////Assert.NotNull(serviceNaesbEventProcessCreateRfc);
            ////Assert.Equal(999, id);
            
        }

        [Fact]
        public void TestProcessConfirmationResponse()
        {
            /////////////////////////////
            ////arrange - mock it up
            /////////////////////////////
            //Mock<INaesbEventProcessRepository> moqNaesbEventProcessRepository = new Mock<INaesbEventProcessRepository>();
            ////moqNaesbEventProcessRepository
            ////    .Setup(x => x.Create(
            ////        new Nomination.Domain.Naesb.NaesbEventProcess
            ////        {
            ////            Cycle = "ID3",
            ////            //DomainData = @"<?xml version=""1.0""?><ConfirmationResponse><GasDayStart>201905012200</GasDayStart><GasDayEnd>201905020900</GasDayEnd><GasDay>2019-05-01T00:00:00</GasDay><Cycle>ID3</Cycle><PartyIndentificaton><PipelineEntityId>046077343</PipelineEntityId><UtilityEntityId>780062605</UtilityEntityId></PartyIndentificaton><Locations><Location><Id>245609</Id><ContractNominations><ContractNomination><Id>SUPPLY</Id><Nominations><Nomination><Id>WE021615357</Id><Quantity>665</Quantity><Unit>Million BTU's</Unit><FlowIndicator>R</FlowIndicator><SolicitedIndicator>S</SolicitedIndicator><Stream><Direction>UP</Direction><EntityId>780062605</EntityId><ContractId>FT0554</ContractId></Stream><NomsContractInfo><EntityId>780062605</EntityId><ContractId>SUPPLY</ContractId></NomsContractInfo></Nomination></Nominations></ContractNomination></ContractNominations></Location></Locations></ConfirmationResponse>",
            ////            DomainData = It.IsAny<string>(),
            ////            //EdiData = @"<?xml version=""1.0""?><ConfirmationResponse><Header><TransactionSetPurposeCode>00</TransactionSetPurposeCode><TransactionIdentifier>MERC_GLG_20190501ID3</TransactionIdentifier><TransactionDate>20190501</TransactionDate><TransactionTypeCode>G3</TransactionTypeCode></Header><ConfirmingParties><ConfirmingParty><EntityIdentifierCode>CNP</EntityIdentifierCode><ConfirmingId>046077343</ConfirmingId></ConfirmingParty><ConfirmingParty><EntityIdentifierCode>CNR</EntityIdentifierCode><ConfirmingId>780062605</ConfirmingId></ConfirmingParty></ConfirmingParties><Nominations><Nomination><DateTimeQualifier>007</DateTimeQualifier><DateTimeFormatQualifier>RDT</DateTimeFormatQualifier><DateTimePeriod>201905012200-201905020900</DateTimePeriod><Cycle><Qualifier>CYI</Qualifier><Indicator>ID3</Indicator></Cycle><Location><GasLocation>LCN</GasLocation><IdCodeQualifier>SV</IdCodeQualifier><Code>245609</Code><ContractSummary><ContractIdentification><IdentifierCode>CNS</IdentifierCode><IdentificationCodeQualifier>1</IdentificationCodeQualifier><IdentificationId>046077343</IdentificationId></ContractIdentification></ContractSummary><NominationDetails><NominationDetail><ConfirmationTrackingId>WE021615357</ConfirmationTrackingId><RelationshipCode>I</RelationshipCode><Quantity>665</Quantity><Unit>BZ</Unit><AdditionalInformations><AdditionalInformation><Indentifier>CFI</Indentifier><Code>R</Code></AdditionalInformation><AdditionalInformation><Indentifier>SUI</Indentifier><Code>S</Code></AdditionalInformation></AdditionalInformations><Identifiers><Identifier><EntityIdCode>78</EntityIdCode><IdCodeQualifier>1</IdCodeQualifier><Code>780062605</Code><IdentifierDetails><IdentifierDetail><Qualifier>KSR</Qualifier><Code>SUPPLY</Code></IdentifierDetail></IdentifierDetails></Identifier><Identifier><EntityIdCode>US</EntityIdCode><IdCodeQualifier>1</IdCodeQualifier><Code>780062605</Code><IdentifierDetails><IdentifierDetail><Qualifier>UP</Qualifier><Code>FT0554</Code></IdentifierDetail></IdentifierDetails></Identifier></Identifiers></NominationDetail></NominationDetails></Location></Nomination></Nominations></ConfirmationResponse>",
            ////            EdiData = It.IsAny<string>(),
            ////            EdiFileName = "MERC_GLGT_CR_20190503115503.xml",
            ////            GasDay = new DateTime(2019, 05, 01),
            ////            Id = 0,
            ////            Pipeline = "GLGT",
            ////            ProcessEnd = null,
            ////            ProcessStart = new DateTime(2018, 11, 13, 14, 15, 00),
            ////            StackTrace = null,
            ////            Type = "CR",
            ////            Utility = "MERC",
            ////            UserId = "xxxx"
            ////        }
            ////    ))
            ////    .Returns(999);
            //moqNaesbEventProcessRepository
            //    .Setup(x => x.Create(It.IsAny<Nomination.Domain.Naesb.NaesbEventProcess>()))
            //    .Returns(999);

            //moqNaesbEventProcessRepository
            //    .Setup(x => x.Update(999, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now)));

            //Mock<IConfirmationResponseRepository> moqConfirmationResponseRepository = new Mock<IConfirmationResponseRepository>();
            //moqConfirmationResponseRepository
            //    .Setup(x => x.Create(
            //        new Nomination.Domain.ConfirmationResponse.ConfirmationResponse
            //        {
            //            Cycle = ""
            //        },
            //        "xxxxx"
            //    ));

            //Mock<INaesbPipelineRepository> moqNaesbPipelineRepository = new Mock<INaesbPipelineRepository>();
            //moqNaesbPipelineRepository
            //    .Setup(x => x.GetByPipelineEntityID("046077343"))
            //    .Returns(new Nomination.Domain.Naesb.NaesbPipeline
            //    {
            //        Pipeline = "GLGT",
            //        PipelineEntityId = "046077343"

            //    });

            //Mock<INaesbUtilityRepository> moqNaesbUtilityRepository = new Mock<INaesbUtilityRepository>();
            //moqNaesbUtilityRepository
            //    .Setup(x => x.GetByUtilityEntityId("780062605"))
            //    .Returns(new Nomination.Domain.Naesb.NaesbUtility
            //    {
            //        Utility = "MERC",
            //        UtilityEntityId = "780062605"
            //    });


            //NaesbPipelineGetByEntityId serviceNeasbPipeline = new NaesbPipelineGetByEntityId(moqNaesbPipelineRepository.Object);
            //NaesbUtilityGetByEntityId serviceNeasbUtility = new NaesbUtilityGetByEntityId(moqNaesbUtilityRepository.Object);
            //ConfirmationResponseCreate serviceConfirmationResponseCreate =
            //    new ConfirmationResponseCreate(
            //        new Nomination.BusinessLayer.Services.ServiceSettings {UserId = "xxxxx"},
            //        moqConfirmationResponseRepository.Object
            //    );

            //NaesbEventProcessCreateCr service = new NaesbEventProcessCreateCr(
            //    new Nomination.BusinessLayer.Services.ServiceSettings {UserId = "xxxxx"},
            //    moqNaesbEventProcessRepository.Object,
            //    serviceConfirmationResponseCreate,
            //    serviceNeasbPipeline,
            //    serviceNeasbUtility
            //);


            //FileInfo file = new FileInfo("../../Files/MERC_GLGT_CR_20190503115503.xml");

            ///////////
            ////act
            ///////////
            //int id = service.Invoke(new DateTime(2018, 11, 13, 14, 15, 00), file);

            ///////////
            ////assert
            ///////////
            //Assert.NotNull(service);
            //Assert.Equal(999, id);
        }

        [Fact]
        public void TestProcessScheduledQuantity()
        {
            ///////////////////////////////
            //////arrange - mock it up
            ///////////////////////////////
            ////Mock<INaesbEventProcessRepository> moqNaesbEventProcessRepository = new Mock<INaesbEventProcessRepository>();
            ////moqNaesbEventProcessRepository
            ////    .Setup(x => x.Create(It.IsAny<Nomination.Domain.Naesb.NaesbEventProcess>()))
            ////    .Returns(999);

            ////moqNaesbEventProcessRepository
            ////    .Setup(x => x.Update(999, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now)));

            ////moqNaesbEventProcessCreateRepository
            ////    .Setup(x => x.Create(999, new KeyValuePair<string, DateTime>("ProcessEnd", DateTime.Now)));

            ////Mock<IScheduledQuantityRepository> moqScheduledQuantityRepository = new Mock<IScheduledQuantityRepository>();
            ////moqScheduledQuantityRepository
            ////    .Setup(x => x.Create(
            ////        It.IsAny<Nomination.Domain.ScheduledQuantities.ScheduledQuantities>(),
            ////        "xxxxx"
            ////    ));

            ////Mock<INaesbPipelineRepository> moqNaesbPipelineRepository = new Mock<INaesbPipelineRepository>();
            ////moqNaesbPipelineRepository
            ////    .Setup(x => x.GetByPipelineEntityID("046077343"))
            ////    .Returns(new Nomination.Domain.Naesb.NaesbPipeline
            ////    {
            ////        Pipeline = "GLGT",
            ////        PipelineEntityId = "046077343"

            ////    });

            ////Mock<INaesbUtilityRepository> moqNaesbUtilityRepository = new Mock<INaesbUtilityRepository>();
            ////moqNaesbUtilityRepository
            ////    .Setup(x => x.GetByUtilityEntityId("780062605"))
            ////    .Returns(new Nomination.Domain.Naesb.NaesbUtility
            ////    {
            ////        Utility = "MERC",
            ////        UtilityEntityId = "780062605"
            ////    });

            ////NaesbPipelineGetByEntityId serviceNeasbPipeline = new NaesbPipelineGetByEntityId(moqNaesbPipelineRepository.Object);
            ////NaesbUtilityGetByEntityId serviceNeasbUtility = new NaesbUtilityGetByEntityId(moqNaesbUtilityRepository.Object);
            ////ScheduledQuantityCreate serviceScheduledQuantityCreate =
            ////    new ScheduledQuantityCreate(
            ////        new Nomination.BusinessLayer.Services.ServiceSettings { UserId = "xxxxx" },
            ////        moqScheduledQuantityRepository.Object
            ////    );

            ////NaesbEventProcessCreateOsq service = new NaesbEventProcessCreateOsq(
            ////    new Nomination.BusinessLayer.Services.ServiceSettings { UserId = "xxxxx" },

            ////    moqNaesbEventProcessRepository.Object,
            ////    serviceScheduledQuantityCreate,
            ////    serviceNeasbPipeline,
            ////    serviceNeasbUtility
            ////);

            ////FileInfo file = new FileInfo("../../Files/MERC_GLGT_OSQ_20190503115503.xml");

            /////////////
            //////act
            /////////////
            ////int id = service.Invoke(new DateTime(2018, 11, 13, 14, 15, 00), file);

            /////////////
            //////assert
            /////////////
            ////Assert.NotNull(service);
            ////Assert.Equal(999, id);
        }
    }
}
