// Create clients and set shared const values outside of the handler.




/**
 * A simple example includes a HTTP get method to get one item by id from a DynamoDB table.
 */

const tickedDal = require('../dal/ticket')
const queryHelper = require('../dal/queryhelper')

function HTTPError (statusCode, message) {
  const error = new Error(message)
  error.statusCode = statusCode
  return error
}

exports.getTicketByIdHandler = async (event) => {
  if (event.httpMethod !== 'GET') {
    throw new Error(`getMethod only accept GET method, you tried: ${event.httpMethod}`);
  }

  // All log statements are written to CloudWatch
  console.info('from code pipeline received new test3:', event);
  
  // Get id from pathParameters from APIGateway because of `/{id}` at template.yml
  const ticketid = event.pathParameters.ticketid;

  const { Ticket } = await tickedDal()
  
  const ticket = await Ticket.findByPk(ticketid)
  if (!ticket) throw new HTTPError(404, `Ticket with id: ${ticketid} was not found`)
 
  // Get the item from the table
 // get ticket by id
 const response = {
    statusCode: 200,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Credentials': true,
    },
    body: JSON.stringify(ticket),
  };
 
 
  // All log statements are written to CloudWatch
  console.info(`response from: ${event.path} statusCode: ${response.statusCode} body: ${response.body}`);
  return response;
}

exports.getTicketsHandler = async (event) => {
  //if (event.httpMethod !== 'GET') {
   // throw new Error(`getMethod only accept GET method, you tried: ${event.httpMethod}`);
  //}
  //const { Ticket } = await tickedDal()
  //const tickets = await Ticket.findAll()
  const tickets= await queryHelper.executeQuery("Select * From tickets")

  
  //const tickets = await 
  // All log statements are written to CloudWatch
  console.info('received new event:', event);
  
  // Get id from pathParameters from APIGateway because of `/{id}` at template.yml
  //const ticketid = event.pathParameters.ticketid;
 
  // Get the item from the table
 // get ticket by id
 
 const response = {
  statusCode: 200,
  headers: {
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Credentials': true,
    'Access-Control-Allow-Methods': "OPTIONS,POST,GET,PUT,DELETE,PATCH",
    'Access-Control-Allow-Headers' :'*'
  },
  body: JSON.stringify(tickets),
};
 
  // All log statements are written to CloudWatch
  console.info(`response from: ${event.path} statusCode: ${response.statusCode} body: ${response.body}`);
  return response;
}

exports.createTicketsHandler = async (event) => {
  if (event.httpMethod !== 'PUT') {
    throw new Error(`getMethod only accept PUT method, you tried: ${event.httpMethod}`);
  }
  
  const { Ticket } = await tickedDal()
  const ticket = await Ticket.create(JSON.parse(event.body))
  // All log statements are written to CloudWatch
  console.info('received:', event);
  
  // Get id from pathParameters from APIGateway because of `/{id}` at template.yml
  //const ticketid = event.pathParameters.ticketid;  
  // Get the item from the table
 // get ticket by id
 
 const response = {
  statusCode: 200,
  headers: {
    'Access-Control-Allow-Origin': '*',
    'Access-Control-Allow-Credentials': true,
    'Access-Control-Allow-Methods': "OPTIONS,POST,GET,PUT,DELETE,PATCH",
    'Access-Control-Allow-Headers' :'*'
  },
  body: JSON.stringify(tickets),
};
 
  // All log statements are written to CloudWatch
  console.info(`response from: ${event.path} statusCode: ${response.statusCode} body: ${response.body}`);
  return response;
}

exports.updateTicketsHandler = async (event) => {
  if (event.httpMethod !== 'POST') {
    throw new Error(`updateMethod only accept POST method, you tried: ${event.httpMethod}`);
  }
  
  const input = JSON.parse(event.body)
  const { Ticket } = await tickedDal()
  const ticket = await Ticket.findById(event.pathParameters.ticketid)
  if (!ticket) throw new HTTPError(404, `Ticket with id: ${ticketid} was not found`)
  if (input.title) ticket.title = input.title
  if (input.description) ticket.description = input.description
  await ticket.save()
 
  const response = {
    statusCode: 200,
    headers: {
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Credentials': true,
      'Access-Control-Allow-Methods': "OPTIONS,POST,GET,PUT,DELETE,PATCH",
      'Access-Control-Allow-Headers' :'*'
    },
    body: JSON.stringify(tickets),
  };
 
  // All log statements are written to CloudWatch
  console.info(`response from: ${event.path} statusCode: ${response.statusCode} body: ${response.body}`);
  return response;
}
