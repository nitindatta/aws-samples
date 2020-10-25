//This function will index document sequences that are added in capo_sequences_table table.

const axios = require('axios');
const AWS = require("aws-sdk");
const indexname=`${process.env.ELASTIC_INDEX_NAME}`;
const indexingurl = `${process.env.ELASTIC_SEARCH_URL}/${indexname}/_doc/`;
const requestTimeout=3000;
const connectTimeout=5000;
//Fields to index from dynamodb table on which stream is enabled. 
const sourcetable={
    keys:["key1","Key2"],
    fieldstoindex: ["field1","field2"]
};

exports.handler = async (event, context) => {

    const elasticconfig = {
        timeout:requestTimeout
    };
    
    AWS.config.update({
        maxRetries: 2,
        httpOptions: {
            timeout: requestTimeout,
            connectTimeout: connectTimeout
            }
    });
    
    var ddb = new AWS.DynamoDB.DocumentClient();
    
    for (const record of event.Records) 
    {
        let docid;
        let docfolder;
        let keys=[];
        let recordkeys=record.dynamodb.Keys;
        //Created elastic documentid based on keys of dynamodb table
        sourcetable.keys.forEach(key => {
            var keyvalue=recordkeys[key][Object.keys(recordkeys[key])[0]];
            keys.push(keyvalue);
        });
        docid=keys.join("_");
        
        console.log('indexing started for document with docid_seq : ' + docid);
        
        //check if record is removed
        if (record.eventName == 'REMOVE') 
        {
            try {
                await axios.delete(indexingurl + docid);
                return 'document removed from elastic index';
            }
            catch (e) {
              console.log(e);
              return 'error in removing document from elastic index';
            }
        }
        else // Record added or update, index in elastic search
        {
            const document = record.dynamodb.NewImage;
            let indexdocument={};
            sourcetable.fieldstoindex.forEach(field => {
                if (field in document)
                {
                    var fieldvalue=document[field][Object.keys(document[field])[0]];
                    indexdocument[field]=fieldvalue;
                }
            });
            
            console.log('Adding document to elastic');
            await axios.put(indexingurl+docid,indexdocument,elasticconfig);
            console.log('docment added successfully in es: '+docid);
            }
    } // records for each loop    
    return "Success";
};