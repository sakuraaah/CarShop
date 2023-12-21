const { createProxyMiddleware } = require('http-proxy-middleware');
const { env } = require('process');

const target = env.ASPNETCORE_HTTPS_PORT ? `https://localhost:${env.ASPNETCORE_HTTPS_PORT}` :
  env.ASPNETCORE_URLS ? env.ASPNETCORE_URLS.split(';')[0] : 'http://localhost:36073';

const context =  [  // TODO
  "/weatherforecast",
  "/api/image",
  "/api/posts",
  "/api/statuses",
  "/api/features",
  "/api/colors",
  "/api/car-classes",
  "/api/marks",
  "/api/categories",
  "/api/rent-categories",
  "/api/body-types",
  "/api/rent-submissions",
  "/api/rent-items",
  "/api/buy-items",
  "/_configuration",
  "/.well-known",
  "/Identity",
  "/connect",
  "/ApplyDatabaseMigrations",
  "/_framework"
];

module.exports = function(app) {
  const appProxy = createProxyMiddleware(context, {
    proxyTimeout: 10000,
    target: target,
    secure: false,
    headers: {
      Connection: 'Keep-Alive'
    }
  });

  app.use(appProxy);
};
