using SkincareProductSalesSystem.Repositories.Database;
using SkincareProductSalesSystem.Repositories.Repositories;

namespace SkincareProductSalesSystem.Repositories
{
    public class UnitOfWork
    {
        private SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext context;
        private BlogPostRepository blogPostRepository;
        private BrandRepository brandRepository;
        private CategoryRepository categoryRepository;
        private CustomerAddressRepository customerAddressRepository;
        private CustomerProfileRepository customerProfileRepository;
        private OrderRepository orderRepository;
        private OrderDetailRepository orderDetailRepository;
        private PaymentRepository paymentRepository;
        private PaymentMethodRepository paymentMethodRepository;
        private ProductRepository productRepository;
        private PromotionRepository promotionRepository;
        private PromotionUsageRepository promotionUsageRepository;
        private ReviewRepository reviewRepository;
        private RoutineProductRepository routineProductRepository;
        private SkinCareRoutineRepository skinCareRoutineRepository;
        private SkinTestRepository skinTestRepository;
        private SkinTestOptionRepository skinTestOptionRepository;
        private SkinTypeRepository skinTypeRepository;
        private UserRepository userRepository;


        public UnitOfWork()
        {
            context ??= new SP25_NET1721_RPR231_PRJ_G1_SkincareProductSalesSystemDBContext();
        }

        public BlogPostRepository BlogPostRepository { get { return blogPostRepository ??= new BlogPostRepository(); } }
        public BrandRepository BrandRepository { get { return brandRepository ??= new BrandRepository(); } }
        public CategoryRepository CategoryRepository { get { return categoryRepository ??= new CategoryRepository(); } }
        public CustomerAddressRepository CustomerAddressRepository { get { return customerAddressRepository ??= new CustomerAddressRepository(); } }
        public CustomerProfileRepository CustomerProfileRepository { get { return customerProfileRepository ??= new CustomerProfileRepository(); } }
        public OrderRepository OrderRepository { get { return orderRepository ??= new OrderRepository(); } }
        public OrderDetailRepository OrderDetailRepository { get { return orderDetailRepository ??= new OrderDetailRepository(); } }
        public PaymentMethodRepository PaymentMethodRepository { get { return paymentMethodRepository ??= new PaymentMethodRepository(); } }
        public ProductRepository ProductRepository { get { return productRepository ??= new ProductRepository(); } }
        public PromotionRepository PromotionRepository { get { return promotionRepository ??= new PromotionRepository(); } }
        public PromotionUsageRepository PromotionUsageRepository { get { return promotionUsageRepository ??= new PromotionUsageRepository(); } }
        public ReviewRepository ReviewRepository { get { return reviewRepository ??= new ReviewRepository(); } }
        public RoutineProductRepository RoutineProductRepository { get { return routineProductRepository ??= new RoutineProductRepository(); } }
        public SkinCareRoutineRepository SkinCareRoutineRepository { get { return skinCareRoutineRepository ??= new SkinCareRoutineRepository(); } }
        public SkinTestRepository SkinTestRepository { get { return skinTestRepository ??= new SkinTestRepository(); } }
        public SkinTestOptionRepository SkinTestOptionRepository { get { return skinTestOptionRepository ??= new SkinTestOptionRepository(); } }
        public PaymentRepository PaymentRepository { get { return paymentRepository ??= new PaymentRepository(); } }

        public SkinTypeRepository SkinTypeRepository { get { return skinTypeRepository ??= new SkinTypeRepository(); } }
        public UserRepository UserRepository { get { return userRepository ??= new UserRepository(); } }
    }
}
