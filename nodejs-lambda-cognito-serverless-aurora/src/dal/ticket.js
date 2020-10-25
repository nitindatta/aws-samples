const dbconnection = require('./dbconnection')
const Sequelize = require('sequelize')
const TicketModel = require('../models/Ticket')
dbconnection.connectDb()
const Ticket = TicketModel(dbconnection.sequelize, Sequelize)
const Models = { Ticket }


module.exports = async () => {
  return Models
}