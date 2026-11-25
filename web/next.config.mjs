/** @type {import('next').NextConfig} */

// The API is reached through this app: the browser only ever talks to one origin, so there is no CORS setup
// and no API address baked into the client. Set API_INTERNAL_URL when building (Docker: http://api:8080).
const api = (process.env.API_INTERNAL_URL || 'http://localhost:5310').replace(/\/$/, '');

const nextConfig = {
  // Self-contained server (node server.js) for containers.
  output: 'standalone',
  distDir: process.env.NEXT_DIST_DIR || '.next',
  async rewrites() {
    return [{ source: '/api/:path*', destination: `${api}/api/:path*` }];
  },
  experimental: {
    optimizePackageImports: ['lucide-react'],
    // Writing a store takes a while behind the proxy.
    proxyTimeout: 300_000,
  },
};

export default nextConfig;
