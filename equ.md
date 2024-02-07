$$
\begin{pmatrix}
v_1 \\
v_2 \\
v_n
\end{pmatrix}
$$
$$

$$

$$
\\ P(\overrightarrow{X},w),  \ \overrightarrow{X}\begin{pmatrix}x\\y\\z\end{pmatrix}, \ P \neq S
$$
$$ 
\\ P'(\overrightarrow{X_{p}},0),  \ \overrightarrow{X_p}\begin{pmatrix}x_p\\y_p\\z_p\end{pmatrix} 
\\ S(0_{3},-1) 
$$

$$
\overrightarrow{SP'} = \begin{pmatrix}x\\y\\z\\w+1\end{pmatrix} = \begin{pmatrix}X\\w+1\end{pmatrix}  \\ 
\overrightarrow{SP} = \begin{pmatrix}x_p\\y_p\\z_p\\1\end{pmatrix} = \begin{pmatrix}X_p\\1\end{pmatrix}
$$
$$
    \overrightarrow{SP'} = \lambda. \overrightarrow{SP} \ \ \ i.e \ \ \ \lambda = w+1 \ \ et \ \ \overrightarrow{X_p} = \frac{\overrightarrow{X}}{w+1} 
$$

$$
\begin{vmatrix}\overrightarrow{X}\end{vmatrix}^{2} + w^{2} = 1
\\\lambda^{2}.\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix} + (\lambda - 1 )^{2} = 1
\\ \lambda^{2}.\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix} + \lambda^{2} -2.\lambda = 0
\\\lambda \neq 0 \ , \ \  \lambda = \frac{2}{\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix}^{2} + 1}
$$

$$
 %\overrightarrow{X} = \frac{2.\overrightarrow{X_p}}{\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix}^{2}+1} \, \ \
 w = \frac{-\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix}^{2}+1}{\begin{vmatrix}\overrightarrow{X_p}\end{vmatrix}^{2}+1}
$$
$$
M \times \begin{pmatrix}x'\\y'\\z'\\w'\end{pmatrix} = \begin{pmatrix}x_p(w+1)\\y_p(w+1)\\z_p(w+1)\\w\end{pmatrix} \, \ \ 
\left\{
    \begin{array}{ll}
        L_1 \cdot (x_1',y_1',z_1',w_1')^{T} = (X_{1p}(w_1+1),w_1)^{T}\\
        L_1 \cdot (x_2',y_2',z_2',w_2')^{T} = (X_{2p}(w_2+1),w_2)^{T}\\
        L_1 \cdot (x_3',y_3',z_3',w_3')^{T} = (X_{3p}(w_3+1),w_3)^{T}\\
        L_1 \cdot (x_4',y_4',z_4',w_4')^{T} = (X_{4p}(w_4+1),w_4)^{T}
    \end{array}
\right.
\ ....
$$