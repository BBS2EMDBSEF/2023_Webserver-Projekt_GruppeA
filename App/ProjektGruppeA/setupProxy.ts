import {createProxyMiddleware} from 'http-proxy-middleware';

module.exports = function (app) {
  app.use(
    '/api', 
    createProxyMiddleware({
      target: 'https://localhost:7204', 
      changeOrigin: true,
    })
  );
};
