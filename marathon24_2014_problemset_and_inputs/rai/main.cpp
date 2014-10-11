// #ifdef DEBUG
// #define _GLIBCXX_DEBUG
// #endif
#include <iostream>
#include <iomanip>
#include <vector>
#include <valarray>
#include <map>
#include <set>
#include <list>
#include <queue>
#include <stack>
#include <bitset>
#include <utility>
#include <numeric>
#include <algorithm>
#include <functional>
#include <complex>
#include <string>
#include <sstream>

#include <cstdio>
#include <cstdlib>
#include <cctype>
#include <cstring>

// these require C++11
#include <unordered_set>
#include <unordered_map>
#include <random>
#include <chrono>

using namespace std;

#define int long long

#define all(c) c.begin(), c.end()
#define repeat(i, n) for (int i = 0; i < static_cast<int>(n); i++)
#define debug(x) #x << "=" << (x)

#ifdef DEBUG
#define _GLIBCXX_DEBUG
#define dump(x) std::cerr << debug(x) << " (L:" << __LINE__ << ")" << std::endl
#else
#define dump(x) 
#endif

typedef complex<double> point;

template<typename T,std::size_t N>
struct _v_traits {using type = std::vector<typename _v_traits<T,N-1>::type>;};
template<typename T>
struct _v_traits<T,1> {using type = std::vector<T>;};
template<typename T,std::size_t N=1>
using vec = typename _v_traits<T,N>::type;

template <typename T>
ostream &operator<<(ostream &os, const vector<T> &vec) {
    os << "[";
    for (const auto &v : vec) {
        os << v << ",";
    }
    os << "]";
    return os;
}

struct v {
    int x,y;
};
signed main() {
    ios::sync_with_stdio(false);
    cin.tie(0);

    int n,m;
    cin >> n >> m;
    // x,y
    vector<v> vertexes;
    vertexes.reserve(n);
    for(int i=0;i<n;i++){
        int x,y;cin >> x >> y;
        vertexes.push_back(v{x,y});
    }
    vector<int> top_vertex;
    bool is_up = true;
    for(int i=0;i<n-1;i++){
        bool will_up = vertexes[i+1].y > vertexes[i].y;
        if(is_up != will_up){
            is_up = will_up;
            top_vertex.push_back(i);
        }
    }
    vector<double> caps;
    vector<double> merge_height;
    for(int i=1;i<top_vertex.size();i+=2){
        // i-1,i,i+1
        int left = top_vertex[i-1];
        int mid = top_vertex[i];
        merge_height.push_back(vertexes[mid].y);
        int right = top_vertex[i+1];

        double t = min(vertexes[left].y,vertexes[right].y);
        if(vertexes[left].y <= vertexes[right].y){
            double cap = 0;
            for(int j=left+1;j<=right;j++){
                double ay = vertexes[j-1].y;
                double ax = vertexes[j-1].x;
                double by = vertexes[j].y;
                double bx = vertexes[j].x;
                if(by > t){
                    // limit x
                    double limit = (t-ay)/(by-ay)*(bx-ax)+ax;
                    cap += ((t-ay))*(limit-ax)/2;
                    break;
                }
                cap += ((t-ay)+(t-by))*(bx-ax)/2;
                dump(cap);
            }
            caps.push_back(cap);
        }else if(vertexes[left].y > vertexes[right].y){
            double cap = 0;
            for(int j=right;j>left;j--){
                double ay = vertexes[j-1].y;
                double ax = vertexes[j-1].x;
                double by = vertexes[j].y;
                double bx = vertexes[j].x;
                if(ay > t){
                    // limit x
                    double limit = (1-(t-by)/(ay-by))*(bx-ax)+ax;
                    cap += ((t-by))*(bx-limit)/2;
                    break;
                }
                cap += ((t-ay)+(t-by))*(bx-ax)/2;
            }
            caps.push_back(cap);
        }
    }

    // current_value,caps,top_vertex
    vector<double> current_value(caps.size());
    for(int query=0;query<m;query++){
        char t;cin >> t;
        if(t == 'R'){ // rain
            int clx,crx,w;
            cin >> clx >> crx >> w;
            for(int i=1;i<top_vertex.size();i+=2){
                int left = top_vertex[i-1];
                int right = top_vertex[i+1];
                int lx = vertexes[left].x;
                int rx = vertexes[right].x;
                if(crx <= lx or
                   clx >= rx) continue;
                double f = w * (min(crx,rx) - max(clx,lx));
                current_value[(i-1)/2] += f;
            }
            for(int i=1;i<top_vertex.size();i+=2){
                int left = top_vertex[i-1];
                int right = top_vertex[i+1];
                int ly = vertexes[left].y;
                int ry = vertexes[right].y;

                int ind = (i-1)/2;
                int si = ind;
                while(current_value[ind] > caps[ind]){
                    double overflow = current_value[ind] - caps[ind];
                    bool recalc_caps = false;
                    if(ly < ry){
                        // its left mountain
                        if(ind == 0){
                            current_value[ind] = caps[ind];
                            break;
                        }
                        if(current_value[ind-1] >= caps[ind-1] - 1e-7){ // already full
                            // merge
                            // left
                            top_vertex[ind*2] = top_vertex[si*2];
                            for(int k=si;k<ind;k++){
                                current_value[ind] += current_value[k];
                            }
                            int rs = si;
                            int re = ind;
                            current_value.erase(current_value.begin()+rs,current_value.begin()+re);
                            caps.erase(caps.begin()+rs,caps.begin()+re);
                            merge_height.erase(merge_height.begin()+rs,merge_height.begin()+re);
                            top_vertex.erase(top_vertex.begin()+rs*2,top_vertex.begin()+re*2);
                            ind -= ind-si;
                            recalc_caps = true;
                        }else{
                            current_value[ind-1] += overflow;
                            current_value[ind] = caps[ind];
                            si = ind;
                            ind--;
                        }
                    }else{
                        if(ind == caps.size()-1){
                            current_value[ind] = caps[ind];
                            break;
                        }
                        if(current_value[ind+1] >= caps[ind+1] - 1e-7){ // already full
                            // merge
                            // right
                            top_vertex[ind*2+2] = top_vertex[si*2+2];
                            for(int k=ind-1;k>=si;k--){
                                current_value[ind] += current_value[k];
                            }
                            int rs = ind;
                            int re = si;
                            current_value.erase(current_value.begin()+rs,current_value.begin()+re);
                            caps.erase(caps.begin()+rs,caps.begin()+re);
                            top_vertex.erase(top_vertex.begin()+rs*2,top_vertex.begin()+re*2);
                            ind -= si-ind;
                            recalc_caps = true;
                        }else{
                            current_value[ind+1] += overflow;
                            current_value[ind] = caps[ind];
                            si = ind;
                            ind++;
                        }
                    }
                    if(recalc_caps){
                        int i = ind*2 + 1;
                        int left = top_vertex[i-1];
                        int mid = top_vertex[i];
                        int right = top_vertex[i+1];

                        double t = min(vertexes[left].y,vertexes[right].y);
                        if(vertexes[left].y <= vertexes[right].y){
                            double cap = 0;
                            for(int j=left+1;j<=right;j++){
                                double ay = vertexes[j-1].y;
                                double ax = vertexes[j-1].x;
                                double by = vertexes[j].y;
                                double bx = vertexes[j].x;
                                if(by > t){
                                    // limit x
                                    double limit = (t-ay)/(by-ay)*(bx-ax)+ax;
                                    cap += ((t-ay))*(limit-ax)/2;
                                    break;
                                }
                                cap += ((t-ay)+(t-by))*(bx-ax)/2;
                            }
                            caps[ind] = cap;
                        }else if(vertexes[left].y > vertexes[right].y){
                            double cap = 0;
                            for(int j=right;j>left;j--){
                                double ay = vertexes[j-1].y;
                                double ax = vertexes[j-1].x;
                                double by = vertexes[j].y;
                                double bx = vertexes[j].x;
                                if(ay > t){
                                    // limit x
                                    double limit = (1-(t-by)/(ay-by))*(bx-ax)+ax;
                                    cap += ((t-by))*(bx-limit)/2;
                                    break;
                                }
                                cap += ((t-ay)+(t-by))*(bx-ax)/2;
                            }
                            caps[ind] = cap;
                        }
                    }
                }
            }
            dump(current_value);
            dump(caps);
        }else{
            int x;cin >> x;
            int y = -1;
            int ret = -1;

            for(int i=0;i<vertexes.size()-1;i++){
                if(vertexes[i].x <= x and x <= vertexes[i+1].x){
                    int dy = vertexes[i+1].y - vertexes[i].y;
                    int dx = vertexes[i+1].x - vertexes[i].x;
                    int cx = x - vertexes[i].x;
                    y = vertexes[i].y + (1.0*cx/dx)*dy;
                }
            }

            for(int i=1;i<top_vertex.size();i+=2){
                int left = top_vertex[i-1];
                int right = top_vertex[i+1];
                int ind = (i-1)/2;
                if(vertexes[left].x <= x and x <= vertexes[right].x){
                    dump(caps[ind]);
                    dump(current_value[ind]);
                    break;
                }
            }
        }
    }
    dump(caps);
    return 0;
}
