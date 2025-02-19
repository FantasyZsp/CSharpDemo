// using System;
// using System.IO;
// using System.Threading.Tasks;
// using Aliyun.OSS;
// using Aliyun.OSS.Common;
// using Xunit;
//
// namespace TestProject.Oss;
//
// public class OssTest
// {
//     [Fact]
//     public async Task TestAsync()
//     {
//         return;
//     }
// }
//
// class Program
// {
//     static async Task Main()
//     {
//         // 阿里云账号 AccessKey 拥有所有 API 的访问权限，风险很高。强烈建议您创建并使用 RAM 用户进行 API 访问或日常运维，请登录 RAM 控制台创建 RAM 用户。
//         string accessKeyId = "<your-access-key-id>";
//         string accessKeySecret = "<your-access-key-secret>";
//         // Endpoint 请参考 https://help.aliyun.com/document_detail/31837.html
//         string endpoint = "<your-endpoint>";
//         string bucketName = "<your-bucket-name>";
//         string objectName = "example-object-name.txt";
//         string localFilePath = "local-file.txt";
//
//         try
//         {
//             // 创建 OSS 客户端实例
//             var client = new OssClient(endpoint, accessKeyId, accessKeySecret);
//
//             // 异步上传文件
//             await UploadObjectAsync(client, bucketName, objectName, localFilePath);
//
//             // 异步下载文件
//             await DownloadObjectAsync(client, bucketName, objectName, localFilePath);
//
//             // 异步删除文件
//             await DeleteObjectAsync(client, bucketName, objectName);
//         }
//         catch (OssException ex)
//         {
//             Console.WriteLine($"阿里云 OSS 操作出错：{ex.ErrorCode} - {ex.Message}");
//         }
//         catch (Exception ex)
//         {
//             Console.WriteLine($"发生未知错误：{ex.Message}");
//         }
//     }
//
//     static async Task UploadObjectAsync(OssClient client, string bucketName, string objectName, string localFilePath)
//     {
//         using (var fileStream = new FileStream(localFilePath, FileMode.Open))
//         {
//             var putObjectRequest = new PutObjectRequest(bucketName, objectName, fileStream);
//             // var putObjectResult = await client.PutObjectAsync(putObjectRequest);
//             Console.WriteLine($"文件上传成功，ETag: {putObjectResult.ETag}");
//         }
//     }
//
//     static async Task DownloadObjectAsync(OssClient client, string bucketName, string objectName, string localFilePath)
//     {
//         var getObjectRequest = new GetObjectRequest(bucketName, objectName);
//         var getObjectResult = await client.GetObjectAsync(getObjectRequest);
//         using (var fileStream = new FileStream(localFilePath, FileMode.Create))
//         {
//             var buffer = new byte[4096];
//             int bytesRead;
//             while ((bytesRead = await getObjectResult.Content.ReadAsync(buffer, 0, buffer.Length)) > 0)
//             {
//                 await fileStream.WriteAsync(buffer, 0, bytesRead);
//             }
//         }
//
//         Console.WriteLine("文件下载成功");
//     }
//
//     static async Task DeleteObjectAsync(OssClient client, string bucketName, string objectName)
//     {
//         await client.DeleteObjectAsync(bucketName, objectName);
//         Console.WriteLine("文件删除成功");
//     }
// }