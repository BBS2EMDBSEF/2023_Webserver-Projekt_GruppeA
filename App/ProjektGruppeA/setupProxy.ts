import {createProxyMiddleware} from 'http-proxy-middleware';

module.exports = function (app) {
  app.use(
    '/api', 
    createProxyMiddleware({
      //target: 'http://localhost:5027', 
      target: 'http://lebedev-systems.de/v.0',
      changeOrigin: true,
    })
  );
};
