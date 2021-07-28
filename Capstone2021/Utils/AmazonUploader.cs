using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using NLog;
using System;

namespace Capstone2021.Utils
{
    public class AmazonUploader
    {
        private static Logger logger;

        public AmazonUploader()
        {
            logger = LogManager.GetCurrentClassLogger();
        }

        public bool sendMyFileToS3(System.IO.Stream localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3)
        {
            IAmazonS3 client = new AmazonS3Client(RegionEndpoint.APSoutheast1);
            TransferUtility utility = new TransferUtility(client);
            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            if (subDirectoryInBucket == "" || subDirectoryInBucket == null)
            {
                request.BucketName = bucketName; //no subdirectory just bucket name  
            }
            else
            {   // subdirectory and bucket name  
                request.BucketName = bucketName + @"/" + subDirectoryInBucket;
            }
            request.Key = fileNameInS3; //file name up in S3  
            request.InputStream = localFilePath;
            try
            {
                utility.Upload(request); //commensing the transfer  
            }
            catch (Exception e)
            {
                logger.Info("Exception occred : " + e.Message + " in AmazonUploader");
                return false;
            }
            return true; //indicate that the file was sent  
        }
    }
}