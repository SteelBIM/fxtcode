namespace FxtDataAcquisition.Domain.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class House
    {
        public Guid? AppId { get; set; }
        public int HouseId { get; set; }

        public int BuildingId { get; set; }
        /// <summary>
        /// ��������
        /// </summary>
        public string HouseName { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? HouseTypeCode { get; set; }
        /// <summary>
        /// ��ʼ��
        /// </summary>
        public int FloorNo { get; set; }
        /// <summary>
        /// ������
        /// </summary>
        public int? EndFloorNo { get; set; }
        /// <summary>
        /// ��Ԫ(�Һ�)
        /// </summary>
        public string UnitNo { get; set; }
        /// <summary>
        /// ���
        /// </summary>
        public decimal? BuildArea { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? FrontCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public int? SightCode { get; set; }
        /// <summary>
        /// ����
        /// </summary>
        public decimal? UnitPrice { get; set; }
        /// <summary>
        /// �۸�ϵ��
        /// </summary>
        public decimal? Weight { get; set; }
        /// <summary>
        /// ��ע
        /// </summary>
        public string Remark { get; set; }
        /// <summary>
        /// ���ͽṹ
        /// </summary>
        public int? StructureCode { get; set; }
        /// <summary>
        /// �ܼ�
        /// </summary>
        public decimal? TotalPrice { get; set; }
        /// <summary>
        /// ��;
        /// </summary>
        public int? PurposeCode { get; set; }
        /// <summary>
        /// �Ƿ�ɹ�
        /// </summary>
        public int? IsEValue { get; set; }

        public int CityID { get; set; }

        public DateTime? CreateTime { get; set; }

        public int? Valid { get; set; }

        public DateTime? SaveDateTime { get; set; }

        public string SaveUser { get; set; }

        public int? FxtCompanyId { get; set; }

        /// <summary>
        /// ���ȷ��
        /// </summary>
        public short? IsShowBuildingArea { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public decimal? InnerBuildingArea { get; set; }
        /// <summary>
        /// ������������
        /// </summary>
        public int? SubHouseType { get; set; }
        /// <summary>
        /// �����������
        /// </summary>
        public decimal? SubHouseArea { get; set; }
        /// <summary>
        /// ����㣨ʵ�ʲ㣩
        /// </summary>
        public string NominalFloor { get; set; }
        /// <summary>
        /// ͨ��ɹ�
        /// </summary>
        public int? VDCode { get; set; }
        /// <summary>
        /// װ��
        /// </summary>
        public int? FitmentCode { get; set; }
        /// <summary>
        /// �Ƿ��г���
        /// </summary>
        public int? Cookroom { get; set; }
        /// <summary>
        /// ��̨��
        /// </summary>
        public int? Balcony { get; set; }
        /// <summary>
        /// ϴ�ּ���
        /// </summary>
        public int? Toilet { get; set; }
        /// <summary>
        /// �������
        /// </summary>
        public int? NoiseCode { get; set; }

        public string Creator { get; set; }

        public int? FxtHouseId { get; set; }

        public int? Status { get; set; }

        public int? TempletId { get; set; }

        public virtual Building Building { get; set; }
        public virtual ICollection<HouseDetails> HouseDetails { get; set; }
        public virtual Templet Templet { get; set; }
    }
}
