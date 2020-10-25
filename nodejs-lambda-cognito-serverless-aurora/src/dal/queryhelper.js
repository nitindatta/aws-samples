const dbconnection = require('./dbconnection')
const { QueryTypes } = require('sequelize');

dbconnection.connectDb()
const sequelizeHandler= dbconnection.sequelize
async function executeQuery(sql){
       
    console.info('SqlQuery: ' + sql);
    const results=  await sequelizeHandler.query(sql, { type: QueryTypes.SELECT })
    
    return results;
}

module.exports.executeQuery = executeQuery