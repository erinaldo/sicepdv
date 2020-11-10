using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SICEpdv
{
    class DriveCloud
    {
        private string azureBlobdriveClientes = "DefaultEndpointsProtocol=https;AccountName=iqscloud;AccountKey=IWI+elhQNwwoz2YF+3HQqzcY6gwD4aNPQ9+BJ7Vl8QCue62ybfIEJrWLkM1j+B9pjnIwtx6z/MPUrV4tqtOZzg==;EndpointSuffix=core.windows.net";  //"DefaultEndpointsProtocol=https;AccountName=sicewebdriveclientes;AccountKey=+OdbO4Rz0DshBhkGLpjakHcaO+igy1DyIh9iqlyjRLg3mqFRQQthIMQE+Gk6N4iv4fSQ2iv0uL3kMG0kn4Kd5w==";
        

        public bool UploadFile(byte[] arquivo, string drive, string descricao, int idCliente, string nomeCliente,string extensao,string arquivoDestino="")
        {
            try
            {
                // Foi acrescentado a letra d=DRIVE para evitar erro quando a chave IQCARD começar com um número

                drive = drive.ToLower();
                
                if(string.IsNullOrEmpty(arquivoDestino))
                 arquivoDestino = Guid.NewGuid().ToString()+extensao;

                try
                {
                    CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes);
                    storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes);
                    CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                    CloudBlobContainer container = blobClient.GetContainerReference(drive);
                    container.CreateIfNotExists();
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Blob });
                    container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Off });
                    CloudBlockBlob blob = container.GetBlockBlobReference(arquivoDestino);
                    System.IO.Stream stream = new System.IO.MemoryStream(arquivo);
                    blob.UploadFromStream(stream);
                    //blob.UploadFromStream(stream, new BlobRequestOptions { Timeout = TimeSpan.FromHours(1) });

                    try
                    {

                        CloudStorageAccount storageTable = CloudStorageAccount.Parse(azureBlobdriveClientes);
                        CloudTableClient tableClient = storageTable.CreateCloudTableClient();
                        CloudTable tabela = tableClient.GetTableReference("DriveClientes");
                        tabela.CreateIfNotExists();

                        DriveRepositorio novo = new DriveRepositorio()
                        {
                            drive = drive,
                            RowKey = arquivoDestino,
                            PartitionKey = "DriveClientes",
                            Timestamp = DateTime.Now,
                            data = DateTime.Now.Date,
                            arquivo = arquivoDestino,
                            uriPath = blob.Uri.AbsoluteUri,
                            descricao = descricao,
                            idCliente = Convert.ToInt16(idCliente),
                            nomeCliente = nomeCliente,
                            operador = "",
                            origem = "",
                            tamanho = blob.Properties.Length,
                            tipoAcesso = "privado"
                        };
                        TableOperation insertOperation = TableOperation.Insert(novo);
                        tabela.Execute(insertOperation);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(ex.Message);
                    }

                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }



        public List<ListaArquivos> ListarArquivos(string drive)
        {
            drive = drive.ToLower();

            // string azureBlobdriveClientes = "DefaultEndpointsProtocol=https;AccountName=sicewebdriveclientes;AccountKey=+OdbO4Rz0DshBhkGLpjakHcaO+igy1DyIh9iqlyjRLg3mqFRQQthIMQE+Gk6N4iv4fSQ2iv0uL3kMG0kn4Kd5w==";


            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());
            CloudTableClient tableClient = storageAccount.CreateCloudTableClient();
            //tableClient.CreateTableIfNotExist("DriveClientes");

            CloudTable tabela = tableClient.GetTableReference("DriveClientes");
            tabela.CreateIfNotExists();

            TableQuery<DriveRepositorio> query = new TableQuery<DriveRepositorio>().Where(TableQuery.GenerateFilterCondition("drive", QueryComparisons.Equal, drive));


            var dados = tabela.ExecuteQuery<DriveRepositorio>(query);



            //TableServiceContext entidade = tableClient.GetDataServiceContext();
            //DataServiceQuery<DriveRepositorio> data = entidade.CreateQuery<DriveRepositorio>("DriveClientes");
            //IEnumerable<DriveRepositorio> dados = data.Where(e => e.PartitionKey == "DriveClientes" && e.data >= DateTime.Now.Date.AddDays(-dias)).AsTableServiceQuery<DriveRepositorio>();

            var queryOrd = dados.OrderByDescending(c => c.data);

            List<ListaArquivos> listagemAr = new List<ListaArquivos>();

            foreach (var item in queryOrd)
            {
                listagemAr.Add(new ListaArquivos
                {
                    drive = item.drive,
                    RowKey = item.RowKey,
                    arquivo = item.arquivo,
                    data = item.data,
                    descricao = item.descricao,
                    idCliente = item.idCliente,
                    nomeCliente = item.nomeCliente,
                    operador = item.operador,
                    origem = item.origem,
                    tamanho = item.tamanho,
                    tipoAcesso = item.tipoAcesso,
                    uriPath = item.uriPath
                });
            };

            return listagemAr;



        }

        public byte[] Download(string drive, string arquivo)
        {
            drive = drive.ToLower();

            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());
            storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
            CloudBlobContainer container = blobClient.GetContainerReference(drive);
            CloudBlockBlob blockBlob = container.GetBlockBlobReference(arquivo);
            using (var memoryStream = new MemoryStream())
            {
                blockBlob.DownloadToStream(memoryStream);               
                return memoryStream.ToArray();
            }
        }

        public bool Apagar(string drive, string id)
        {
            drive = drive.ToLower();
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());
                storageAccount = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());
                try
                {
                    CloudStorageAccount storageTable = CloudStorageAccount.Parse(azureBlobdriveClientes.ToString());

                    CloudTableClient tableClient = storageTable.CreateCloudTableClient();

                    CloudTable table = tableClient.GetTableReference("DriveClientes");

                    TableOperation retrieveOperation = TableOperation.Retrieve<DriveRepositorio>("DriveClientes", id);

                    TableResult retrieveResult = table.Execute(retrieveOperation);

                    DriveRepositorio deleteEntity = (DriveRepositorio)retrieveResult.Result;

                    TableOperation deleteOperation = TableOperation.Delete(deleteEntity);

                    table.Execute(deleteOperation);
                }
                catch (Exception ex)
                {

                    throw new Exception(ex.Message);
                }


                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = blobClient.GetContainerReference(drive);
                container.SetPermissions(new BlobContainerPermissions { PublicAccess = BlobContainerPublicAccessType.Container });
                CloudBlockBlob blob = container.GetBlockBlobReference(id);
                blob.DeleteIfExists();
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

    }



    public class DriveRepositorio : TableEntity
    {
        public string drive { get; set; }
        public string arquivo { get; set; }
        public string uriPath { get; set; }
        public int idCliente { get; set; }
        public string nomeCliente { get; set; }
        public DateTime data { get; set; }
        public string descricao { get; set; }
        public string operador { get; set; }
        public string origem { get; set; }
        public double tamanho { get; set; }
        public string tipoAcesso { get; set; }
    }

    [DataContract]
    public class ListaArquivos
    {
        [DataMember]
        public string drive { get; set; }
        [DataMember]
        public string RowKey { get; set; }
        [DataMember]
        public string arquivo { get; set; }
        [DataMember]
        public string uriPath { get; set; }
        [DataMember]
        public int idCliente { get; set; }
        [DataMember]
        public string nomeCliente { get; set; }
        [DataMember]
        public DateTime data { get; set; }
        [DataMember]
        public string descricao { get; set; }
        [DataMember]
        public string operador { get; set; }
        [DataMember]
        public string origem { get; set; }
        [DataMember]
        public double tamanho { get; set; }
        [DataMember]
        public string tipoAcesso { get; set; }
    }


}
