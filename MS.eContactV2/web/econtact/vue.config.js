const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
    transpileDependencies: true,
    devServer: {
        //https: {
        //    key: fs.readFileSync(keyFilePath),
        //    cert: fs.readFileSync(certFilePath),
        //},
        // proxy: 'http://localhost:8006',
        // proxy: {
        //     '^/api/v1/': {
        //         target: 'https://localhost:7133'
        //     }
        //     // '^/api/v1/customers': {
        //     //     target: 'http://localhost:5003'
        //     // }
        // },
        port: 5005
    },
    pages: {
        index: {
            // entry for the page
            entry: 'src/main.js?v=1',
            // the source template
            template: 'public/index.html',
            // output as dist/index.html
            filename: 'index.html',
            // when using title option,
            // template title tag needs to be <title><%= htmlWebpackPlugin.options.title %></title>
            title: 'LỚP A1 (2004-2007) - TRƯỜNG THPT TỨ SƠN',
            // chunks to include on this page, by default includes
            // extracted common chunks and vendor chunks.
            chunks: ['chunk-vendors', 'chunk-common', 'index']
        },
    }
})