
const DataApi = require('sequelize-aws-data-api-connector')
const Sequelize = require('sequelize')

const sequelize = new Sequelize({dialect:'mysql'}); // your sequelize instance
const resourceArn = 'arn:aws:rds:eu-central-1:899885580749:cluster:ticketapp-rds-cluster';
const secretArn = 'arn:aws:secretsmanager:eu-central-1:899885580749:secret:dev-AuroraUserSecret-i4o5k3';
const database = 'ticketapp_db'; // database
const region = 'eu-central-1'; //region
const verbose = false; // true to log all queries and other data. Default if false


/* module.exports = async () => {
    DataApi.enableDataAPI(sequelize, { resourceArn, secretArn, database, region, verbose });
    await sequelize.sync()
    await sequelize.authenticate()
} */

 async function connectDb(){
    
    DataApi.enableDataAPI(sequelize, { resourceArn, secretArn, database, region, verbose });
    await sequelize.sync()
    await sequelize.authenticate()
} 


sequelize

module.exports.sequelize = sequelize
module.exports.connectDb = connectDb