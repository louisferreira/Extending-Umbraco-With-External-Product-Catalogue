using CodeLinq.Data.Services.Models;
using CodeLinq.Data.ServicesTests.TestEntities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLinq.Data.ServicesTests.Helper
{
    public class DataFactory
    {
        private static string jsonProd = @"[
	            {
                    ""Name"": ""Monkfish Fresh - Skin Off"",
                    ""Sku"": ""49999-845"",
                    ""Description"": ""Other antepartum hemorrhage, second trimester"",
                    ""Price"": 184.40,
                    ""OutOfStock"": false,
                    ""Id"": ""b4f21064-3c3f-459a-b2e4-77e982b3fe83""
                }, {
                    ""Name"": ""Oil - Food, Lacquer Spray"",
                    ""Sku"": ""59316-108"",
                    ""Description"": ""War operations involving fragments of improvised explosive device [IED], civilian, subsequent encounter"",
                    ""Price"": 868.13,
                    ""OutOfStock"": false,
                    ""Id"": ""c6567fbe-7dec-41ea-bdea-8e58d01ba46e""
                }, {
                    ""Name"": ""Soup - Knorr, Country Bean"",
                    ""Sku"": ""64058-154"",
                    ""Description"": ""Driver of pick-up truck or van injured in collision with fixed or stationary object in traffic accident, subsequent encounter"",
                    ""Price"": 705.06,
                    ""OutOfStock"": true,
                    ""Id"": ""78992e1c-ffdc-4591-a9ac-617602276a77""
                }, {
                    ""Name"": ""Beef - Kobe Striploin"",
                    ""Sku"": ""54868-5139"",
                    ""Description"": ""Other specified injury of portal vein, subsequent encounter"",
                    ""Price"": 223.58,
                    ""OutOfStock"": true,
                    ""Id"": ""b0fc32f3-e483-4ccc-bb07-ec0465c09482""
                }, {
                    ""Name"": ""Taro Leaves"",
                    ""Sku"": ""55504-9003"",
                    ""Description"": ""Acute suppurative otitis media with spontaneous rupture of ear drum, recurrent, bilateral"",
                    ""Price"": 746.80,
                    ""OutOfStock"": false,
                    ""Id"": ""734ff155-8ad6-4f65-98c5-c18268d66878""
                }
            ]";
        private static string jsonCat = @"[{
                    ""Name"": ""Test Category 1"",
                    ""Description"": ""Test Category 1"",
                    ""ParentCategoryId"": null,
                    ""Id"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564""
                }, {
                    ""Name"": ""Test Category 2"",
                    ""Description"": ""Test Category 2"",
                    ""ParentCategoryId"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564"",
                    ""Id"": ""b4fb342c-5807-4716-bb58-386b4894dc4a""
                }, {
                    ""Name"": ""Test Category 3"",
                    ""Description"": ""Test Category 3"",
                    ""ParentCategoryId"": ""b4fb342c-5807-4716-bb58-386b4894dc4a"",
                    ""Id"": ""33f76c00-7e72-4f67-8ece-38681ce88bc3""
                }]";
        private static string jsonCatProd = @"[
	                {
		                ""Id"": ""66ab3eef-6b75-4cc4-8a0b-15a77c89c2c8"",
		                ""CategoryId"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564"",
		                ""ProductId"": ""b4f21064-3c3f-459a-b2e4-77e982b3fe83""
	                }, 
	                {
		                ""Id"": ""422a372a-295c-4722-816b-5a011297bc56"",
		                ""CategoryId"": ""b4fb342c-5807-4716-bb58-386b4894dc4a"",
		                ""ProductId"": ""c6567fbe-7dec-41ea-bdea-8e58d01ba46e""
	                }, 
	                {
		                ""Id"": ""46d175f1-0513-4798-8ffa-4814cca85f6c"",
		                ""CategoryId"": ""33f76c00-7e72-4f67-8ece-38681ce88bc3"",
		                ""ProductId"": ""78992e1c-ffdc-4591-a9ac-617602276a77""
	                }, 
	                {
		                ""Id"": ""cea930df-7a19-4638-977d-bb1ea0555fb4"",
		                ""CategoryId"": ""33f76c00-7e72-4f67-8ece-38681ce88bc3"",
		                ""ProductId"": ""b0fc32f3-e483-4ccc-bb07-ec0465c09482""
	                }, 
	                {
		                ""Id"": ""4e705153-3c92-411e-ab6d-1b73fab9e432"",
		                ""CategoryId"": ""33f76c00-7e72-4f67-8ece-38681ce88bc3"",
		                ""ProductId"": ""734ff155-8ad6-4f65-98c5-c18268d66878""
	                }, 
	                {
		                ""Id"": ""770f82f8-3b44-4191-a1b2-557067d71722"",
		                ""CategoryId"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564"",
		                ""ProductId"": ""b0fc32f3-e483-4ccc-bb07-ec0465c09482""
	                }, 
	                {
		                ""Id"": ""fe04a636-f873-4d44-9b74-be7075d8442c"",
		                ""CategoryId"": ""b4fb342c-5807-4716-bb58-386b4894dc4a"",
		                ""ProductId"": ""734ff155-8ad6-4f65-98c5-c18268d66878""
	                }
                ]";
        private static string jsonMedia = @"[{
        ""Id"": ""75750707-f77d-4899-85e1-cedae9ac8bff"",
        ""EntityId"": ""b4f21064-3c3f-459a-b2e4-77e982b3fe83"",
        ""MediaType"": 3,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 2786,
        ""Name"": ""Libero.mp3"",
        ""FileExtension"": ""DictumstEtiamFaucibus.mov"",
        ""MimeType"": ""video/mpeg""
    }, {
        ""Id"": ""f038621c-84c3-413b-a72f-74d08ce221e8"",
        ""EntityId"": ""b4f21064-3c3f-459a-b2e4-77e982b3fe83"",
        ""MediaType"": 4,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 2266,
        ""Name"": ""FusceCongue.mp3"",
        ""FileExtension"": ""FringillaRhoncusMauris.mpeg"",
        ""MimeType"": ""audio/x-mpeg-3""
    }, {
        ""Id"": ""c7e45d7d-2c5d-4cde-b4a6-512a56514926"",
        ""EntityId"": ""c6567fbe-7dec-41ea-bdea-8e58d01ba46e"",
        ""MediaType"": 2,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 4222,
        ""Name"": ""TurpisAdipiscingLorem.avi"",
        ""FileExtension"": ""AcNibh.mp3"",
        ""MimeType"": ""application/x-troff-msvideo""
    }, {
        ""Id"": ""5e062133-e180-4903-96d0-7a321fd63c89"",
        ""EntityId"": ""b0fc32f3-e483-4ccc-bb07-ec0465c09482"",
        ""MediaType"": 1,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 5349,
        ""Name"": ""Consequat.mpeg"",
        ""FileExtension"": ""Mauris.mov"",
        ""MimeType"": ""video/mpeg""
    }, {
        ""Id"": ""f4ef8a58-468c-4be1-8ae9-ab8a7c223954"",
        ""EntityId"": ""734ff155-8ad6-4f65-98c5-c18268d66878"",
        ""MediaType"": 4,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 4330,
        ""Name"": ""SemperSapien.avi"",
        ""FileExtension"": ""Fusce.mov"",
        ""MimeType"": ""video/x-msvideo""
    }, {
        ""Id"": ""5efcaa74-4d81-4daf-af92-e098d648a16a"",
        ""EntityId"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564"",
        ""MediaType"": 3,
        ""EntityType"": 1,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 5900,
        ""Name"": ""NibhQuisque.mpeg"",
        ""FileExtension"": ""UltricesErat.mp3"",
        ""MimeType"": ""video/mpeg""
    }, {
        ""Id"": ""5ffd1327-3182-4733-b452-cebf5dd304f4"",
        ""EntityId"": ""720f448a-22b8-439b-8cd2-3a5b0ca21564"",
        ""MediaType"": 3,
        ""EntityType"": 1,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 7867,
        ""Name"": ""AmetLobortis.mp3"",
        ""FileExtension"": ""UtMassa.mpeg"",
        ""MimeType"": ""video/x-mpeg""
    }, {
        ""Id"": ""e3d8805e-e15c-485b-b933-e650979105ca"",
        ""EntityId"": ""33f76c00-7e72-4f67-8ece-38681ce88bc3"",
        ""MediaType"": 4,
        ""EntityType"": 1,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 8757,
        ""Name"": ""Dui.avi"",
        ""FileExtension"": ""ConvallisNunc.mpeg"",
        ""MimeType"": ""application/x-troff-msvideo""
    }, {
        ""Id"": ""b737a755-b1ff-4dd1-9184-e21c12faa1f8"",
        ""EntityId"": ""c6567fbe-7dec-41ea-bdea-8e58d01ba46e"",
        ""MediaType"": 3,
        ""EntityType"": 2,
        ""Uri"": ""http://dummyimage.com/250x250.png"",
        ""FileSize"": 8407,
        ""Name"": ""Lacinia.avi"",
        ""FileExtension"": ""UltricesPhasellusId.avi"",
        ""MimeType"": ""video/avi""
    }]";

        private static Dictionary<Type, Func<object>> creators = new Dictionary<Type, Func<object>>()
        {
            [typeof(Product)] = () =>
            {
                var list = JsonConvert.DeserializeObject<List<Product>>(jsonProd);
                list?.ForEach(p => p.Id = Guid.Parse(p.Id.ToString()));
                return list;
            },
            [typeof(Category)] = () =>
            {
                var list = JsonConvert.DeserializeObject<List<Category>>(jsonCat);
                list.ForEach(c =>
                {
                    c.Id = Guid.Parse(c.Id.ToString());
                    if (c.ParentCategoryId != null)
                    {
                        if(Guid.TryParse(c.ParentCategoryId.ToString(), out var parentId))
                        {
                            c.ParentCategoryId = parentId;
                        }
                    }
                });
                return list;
            },
            [typeof(CategoryProduct)] = () =>
            {
                var list = JsonConvert.DeserializeObject<List<CategoryProduct>>(jsonCatProd);
                list?.ForEach(x => {
                    x.Id = Guid.Parse(x.Id.ToString());
                    if(Guid.TryParse(x.CategoryId.ToString(), out var newCatId))
                    {
                        x.CategoryId = newCatId;
                    }
                    if (Guid.TryParse(x.ProductId.ToString(), out var newProdId))
                    {
                        x.ProductId = newProdId;
                    }
                });
                return list;
            },
            [typeof(Media)] = () => {
                var list = JsonConvert.DeserializeObject<List<Media>>(jsonMedia);
                list?.ForEach(x =>
                {
                    x.Id = Guid.Parse(x.Id.ToString());
                    x.EntityId = Guid.Parse(x.EntityId.ToString());
                });
                return list;
            }
        };

        public static List<T> SeedDummyData<T>()
        {
            return (List<T>)creators[typeof(T)]();
        }

    }
}
